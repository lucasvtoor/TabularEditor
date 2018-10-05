﻿using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabularEditor.TextServices;
using TabularEditor.TOMWrapper.Undo;

namespace TabularEditor.TOMWrapper.Utils
{
    internal static class FormulaFixup
    {
        /// <summary>
        /// Changes all references to object "obj", to reflect the new name of the object.
        /// </summary>
        /// <param name="obj"></param>
        public static void DoFixup(IDaxObject obj)
        {
            foreach (var d in obj.ReferencedBy.ToList())
            {
                d.DependsOn.UpdateRef(obj);
            }
        }

        private static TabularModelHandler Handler { get { return TabularModelHandler.Singleton; } }
        private static Model Model { get { return TabularModelHandler.Singleton.Model; } }

        /// <summary>
        /// This method can be called to obtain a DependsOnList for any DAX expression. This is useful when the
        /// user is currently editing the DAX expression of an object, but the expression has not been saved to
        /// the object.
        /// </summary>
        public static DependsOnList GetDependencies(IDaxDependantObject expressionObj, string dax, DAXProperty prop)
        {
            var dependsOn = new DependsOnList(null);
            ParseExpression(dax, expressionObj, prop, dependsOn);
            return dependsOn;
        }

        private static void ParseExpression(string dax, IDaxDependantObject expressionObj, DAXProperty prop, DependsOnList dependsOn = null)
        {
            var lexer = new DAXLexer(new DAXCharStream(dax, false));
            lexer.RemoveErrorListeners();
            var tokens = lexer.GetAllTokens();

            IToken lastTableRef = null;
            int startTableIndex = 0;

            for (var i = 0; i < tokens.Count; i++)
            {
                var tok = tokens[i];
                switch (tok.Type)
                {
                    case DAXLexer.TABLE:
                    case DAXLexer.TABLE_OR_VARIABLE:
                        if (i < tokens.Count - 1 && tokens[i + 1].Type == DAXLexer.COLUMN_OR_MEASURE)
                        {
                            // Keep the token reference, as the next token should be column (fully qualified).
                            lastTableRef = tok;
                            startTableIndex = tok.StartIndex;
                        }
                        else
                        {
                            // Table referenced directly, don't save the reference for the next token.
                            lastTableRef = null;
                        }

                        if (Model.Tables.Contains(tok.Text.NoQ(true)))
                        {
                            if (dependsOn != null) dependsOn.Add(Model.Tables[tok.Text.NoQ(true)], prop, tok.StartIndex, tok.StopIndex, true);
                            else expressionObj.AddDep(Model.Tables[tok.Text.NoQ(true)], prop, tok.StartIndex, tok.StopIndex, true);
                        }
                        else
                        {
                            // Invalid reference (no table with that name) or possibly a variable or function ref
                        }
                        break;
                    case DAXLexer.COLUMN_OR_MEASURE:
                        // Referencing a table just before the object reference
                        if (lastTableRef != null)
                        {
                            var tableName = lastTableRef.Text.NoQ(true);
                            lastTableRef = null;
                            if (!Model.Tables.Contains(tableName)) return; // Invalid reference (no table with that name)

                            var table = Model.Tables[tableName];
                            // Referencing a column on a specific table
                            if (table.Columns.Contains(tok.Text.NoQ()))
                            {
                                if (dependsOn != null) dependsOn.Add(table.Columns[tok.Text.NoQ()], prop, startTableIndex, tok.StopIndex, true);
                                else expressionObj.AddDep(table.Columns[tok.Text.NoQ()], prop, startTableIndex, tok.StopIndex, true);
                            }
                            // Referencing a measure on a specific table
                            else if (table.Measures.Contains(tok.Text.NoQ()))
                            {
                                if (dependsOn != null) dependsOn.Add(table.Measures[tok.Text.NoQ()], prop, startTableIndex, tok.StopIndex, true);
                                else expressionObj.AddDep(table.Measures[tok.Text.NoQ()], prop, startTableIndex, tok.StopIndex, true);
                            }
                        }
                        // No table reference before the object reference
                        else
                        {
                            var table = (expressionObj as ITabularTableObject)?.Table ?? (expressionObj as RLSFilterExpression)?.Table;
                            // Referencing a column without specifying a table (assume column in same table):
                            if (table != null && table.Columns.Contains(tok.Text.NoQ()))
                            {
                                if (dependsOn != null) dependsOn.Add(table.Columns[tok.Text.NoQ()], prop, tok.StartIndex, tok.StopIndex, false);
                                else expressionObj.AddDep(table.Columns[tok.Text.NoQ()], prop, tok.StartIndex, tok.StopIndex, false);
                            }
                            // Referencing a measure or column without specifying a table
                            else
                            {
                                Measure m = null;
                                if (table != null && table.Measures.Contains(tok.Text.NoQ())) m = table.Measures[tok.Text.NoQ()];
                                else
                                    m = Model.Tables.FirstOrDefault(t => t.Measures.Contains(tok.Text.NoQ()))?.Measures[tok.Text.NoQ()];

                                if (m != null)
                                {
                                    if (dependsOn != null) dependsOn.Add(m, prop, tok.StartIndex, tok.StopIndex, false);
                                    else expressionObj.AddDep(m, prop, tok.StartIndex, tok.StopIndex, false);
                                }
                            }
                        }
                        break;
                    default:
                        lastTableRef = null;
                        break;
                }
            }
        }

        public static void ClearDependsOn(IDaxDependantObject expressionObj)
        {
            foreach (var d in expressionObj.DependsOn.Keys) d.ReferencedBy.Remove(expressionObj);
            expressionObj.DependsOn.Clear();
        }

        public static void BuildDependencyTree(IDaxDependantObject expressionObj)
        {
            if (Handler.EoB_PostponeOperations) { Handler.EoB_RequireRebuildDependencyTree = true; return; }

            ClearDependsOn(expressionObj);

            foreach (var prop in expressionObj.GetDAXProperties())
            {
                var dax = expressionObj.GetDAX(prop) ?? "";
                ParseExpression(dax, expressionObj, prop);
            }
        }

        public static void BuildDependencyTree()
        {
            if (Handler.EoB_PostponeOperations) { Handler.EoB_RequireRebuildDependencyTree = true; return; }

            var sw = new Stopwatch();
            sw.Start();

            foreach (var eo in Model.Tables.SelectMany(t => t.GetChildren()).Concat(Model.Tables).OfType<IDaxDependantObject>())
            {
                BuildDependencyTree(eo);
            }
            foreach (var role in Model.Roles)
            {
                foreach(var table in Model.Tables.Where(t => !string.IsNullOrWhiteSpace(role.RowLevelSecurity[t])))
                {
                    var rfe = RLSFilterExpression.Get(role, table);
                    BuildDependencyTree(rfe);
                }
            }

            sw.Stop();

            Console.WriteLine("Dependency tree built in {0} ms", sw.ElapsedMilliseconds);
        }
    }
}
