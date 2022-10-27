using System;
using System.Text;

namespace TabularEditor.TOMWrapper
{
    #region Classes

    public class DaxBool : Dax
    {
        public DaxBool(string s) : base(s)
        {
        }

        public DaxBool(bool b) : base(b.ToString())
        {
        }
    }

    public class DaxColumnArray : Dax
    {
        public DaxColumnArray(string[] s) : base(string.Join(",", s))
        {
        }

        public DaxColumnArray(string s) : base(s)
        {
        }
    }

    public class DaxFilterArray : Dax

    {
        public DaxFilterArray(string[] s) : base(string.Join(",", s))
        {
        }

        public DaxFilterArray(string s) : base(s)
        {
        }
    }

    public class DaxTable : Dax
    {
        public DaxTable(string s) : base(s)
        {
        }

        public DaxTable(Table t) : base(t.DaxObjectFullName)
        {
        }
    }

    public class DaxColumn : Dax
    {
        public DaxColumn(string s) : base(s)
        {
        }

        public DaxColumn(Column c) : base(c.DaxObjectFullName)
        {
        }
    }

    public class DaxInt : Dax
    {
        public DaxInt(string s) : base(s)
        {
        }

        public DaxInt(int i) : base(i.ToString())
        {
        }
    }

    public class DaxFloat : Dax
    {
        public DaxFloat(string s) : base(s)
        {
        }

        public DaxFloat(float f) : base(f.ToString())
        {
        }
    }

    public class DaxDouble : Dax
    {
        public DaxDouble(string s) : base(s)
        {
        }

        public DaxDouble(double d) : base(d.ToString())
        {
        }
    }

    public class DaxVoid : Dax
    {
        public DaxVoid(string s) : base(s)
        {
        }
    }

    public class DaxExpression : Dax
    {
        public DaxExpression(string s) : base(s)
        {
        }
    }

    public class DaxString : Dax
    {
        public DaxString(string s) : base(s)
        {
        }
    }

    public class DaxFilter : DaxExpression
    {
        public DaxFilter(string s) : base(s)
        {
        }
    }

    public class DaxValue : Dax
    {
        public DaxValue(string s) : base(s)
        {
        }
    }

    public class DaxDateTime : Dax
    {
        public DaxDateTime(string s) : base(s)
        {
        }
    }

    public class Dax
    {
        public string Content;

        public Dax(string s)
        {
            Content = s;
        }

        public override string ToString()
        {
            return Content;
        }
    }

    #endregion

    public class StatementBuilder
    {
        private string _inner;

        public StatementBuilder(Func<StatementBuilder, string> statements)
        {
            _inner = statements.Invoke(this);
        }

        public string Run()
        {
            return _inner;
        }


        #region AggregateFunctions

        /*
         * https://learn.microsoft.com/en-us/dax/approximate-distinctcount-function-dax
         */
        public DaxInt ApproximateDistinctCount(DaxString columnName)
        {
            return new DaxInt($"APPROXIMATEDISTINCTCOUNT({columnName}");
        }

        public DaxInt ApproximateDistinctCount(string columnName)
        {
            return ApproximateDistinctCount(new DaxString(columnName));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/average-function-dax
         */
        public DaxDouble Average(DaxString columnName)
        {
            return new DaxDouble($"AVERAGE({columnName})");
        }

        public DaxDouble Average(string columnName)
        {
            return Average(new DaxString(columnName));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/averagea-function-dax
         */
        public DaxDouble AverageColumnArithmeticMean(DaxColumn column)
        {
            return new DaxDouble($"AVEREGEA({column})");
        }

        public DaxDouble AverageColumnArithmeticMean(string column)
        {
            return AverageColumnArithmeticMean(new DaxColumn(column));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/averagex-function-dax
         */
        public DaxDouble AverageTableArithmeticMean(DaxTable table, DaxExpression expression)
        {
            return new DaxDouble($"AVERAGEX({table},{expression})");
        }

        public DaxDouble AverageTableArithmeticMean(string table, string expression)
        {
            return AverageTableArithmeticMean(new DaxTable(table), new DaxExpression(expression));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/count-function-dax
         * https://learn.microsoft.com/en-us/dax/counta-function-dax
         */
        public DaxInt Count(DaxColumn column, bool countBooleans)
        {
            return new DaxInt(countBooleans ? $"COUNTA({column})" : $"COUNT({column})");
        }

        public DaxInt Count(string column, bool countBooleans)
        {
            return Count(new DaxColumn(column), countBooleans);
        }

        /*
         * https://learn.microsoft.com/en-us/dax/countax-function-dax
         */
        public DaxInt CountTable(DaxTable table, DaxExpression expression)
        {
            return new DaxInt($"COUNTAX({table},{expression})");
        }

        public DaxInt CountTable(string table, string expression)
        {
            return CountTable(new DaxTable(table), new DaxExpression(expression));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/countblank-function-dax
         */
        public DaxInt CountBlank(DaxColumn column)
        {
            return new DaxInt($"COUNTBLANK({column})");
        }

        public DaxInt CountBlank(string column)
        {
            return CountBlank(new DaxColumn(column));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/countrows-function-dax
         */
        public DaxInt CountRows(DaxTable table)
        {
            return new DaxInt($"COUNTROWS({table})");
        }

        public DaxInt CountRows(string table)
        {
            return CountRows(new DaxTable(table));
        }

        public DaxInt CountRows()
        {
            return new DaxInt($"COUNTROWS()");
        }

        /*
         * https://learn.microsoft.com/en-us/dax/countx-function-dax
         */
        public DaxInt CountColumnDaxInTable(DaxTable table, DaxExpression expression)
        {
            return new DaxInt($"COUNTX({table},{expression})");
        }

        public DaxInt CountColumnDaxInTable(string table, string expression)
        {
            return CountColumnDaxInTable(new DaxTable(table), new DaxExpression(expression));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/distinctcount-function-dax
         */
        public DaxString CountDistinct(DaxColumn column, bool countBlank)
        {
            return new DaxString(countBlank ? $"DISTINCTCOUNT({column})" : $"DISTINCTCOUNTNOBLANK({column})");
        }

        public DaxString CountDistinct(string column, bool countBlank)
        {
            return CountDistinct(new DaxColumn(column), countBlank);
        }

        /*
         * https://learn.microsoft.com/en-us/dax/max-function-dax
         */
        public DaxValue Max(DaxColumn column, bool countBoolean)
        {
            return new DaxValue(countBoolean ? $"MAXA({column})" : $"MAX({column})");
        }

        public DaxValue Max(string column, bool countBoolean)
        {
            return Max(new DaxColumn(column), countBoolean);
        }

        /*
        * https://learn.microsoft.com/en-us/dax/max-function-dax
        */
        public DaxValue Max(DaxExpression expression1, DaxExpression expression2)
        {
            return new DaxValue($"MAX({expression1},{expression2})");
        }

        public DaxValue Max(string expression1, string expression2)
        {
            return Max(new DaxExpression(expression1), new DaxExpression(expression1));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/maxx-function-dax
         */
        public DaxValue MaxPerRow(DaxTable table, DaxExpression expression)
        {
            return new DaxValue($"MAXX({table},{expression})");
        }

        public DaxValue MaxPerRow(string table, string expression)
        {
            return MaxPerRow(new DaxTable(table), new DaxExpression(expression));
        }

        public DaxValue Min(DaxColumn column, bool countBoolean)
        {
            return new DaxValue(countBoolean ? $"MINA({column})" : $"MIN({column})");
        }

        public DaxValue Min(string column, bool countBoolean)
        {
            return Min(new DaxColumn(column), countBoolean);
        }

        /*
        * https://learn.microsoft.com/en-us/dax/max-function-dax
        */
        public DaxValue Min(DaxExpression expression1, DaxExpression expression2)
        {
            return new DaxValue($"MIN({expression1},{expression2})");
        }

        public DaxValue Min(string expression1, string expression2)
        {
            return Min(new DaxExpression(expression1), new DaxExpression(expression2));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/maxx-function-dax
         */
        public DaxValue MinPerRow(DaxTable table, DaxExpression expression)
        {
            return new DaxValue($"MINX({table},{expression})");
        }

        public DaxValue MinPerRow(string table, string expression)
        {
            return MinPerRow(new DaxTable(table), new DaxExpression(expression));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/product-function-dax
         */
        public DaxDouble Product(DaxColumn column)
        {
            return new DaxDouble($"PRODUCT({column})");
        }

        public DaxDouble Product(string column)
        {
            return Product(new DaxColumn(column));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/productx-function-dax
         */
        public DaxDouble ProductPerRow(DaxTable table, DaxExpression expression)
        {
            return new DaxDouble($"PRODUCTX({table},{expression})");
        }

        public DaxDouble ProductPerRow(string table, string expression)
        {
            return ProductPerRow(new DaxTable(table), new DaxExpression(expression));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/sum-function-dax
         */
        public DaxDouble Sum(DaxColumn column)
        {
            return new DaxDouble($"SUM({column})");
        }

        public DaxDouble Sum(string column)
        {
            return Sum(new DaxColumn(column));
        }

        public DaxDouble SumPerRow(DaxTable table, DaxExpression expression)
        {
            return new DaxDouble($"SUMX({table},{expression})");
        }

        public DaxDouble SumPerRow(string table, string expression)
        {
            return SumPerRow(new DaxTable(table), new DaxExpression(expression));
        }

        #endregion

        #region DateTimeFunctions

        /*
         * https://learn.microsoft.com/en-us/dax/calendar-function-dax
         */
        public DaxTable Calendar(DaxDateTime startDate, DaxDateTime endDate)
        {
            return new DaxTable($"CALENDAR({startDate},{endDate})");
        }

        public DaxTable Calendar(string startDate, string endDate)
        {
            return Calendar(new DaxDateTime(startDate), new DaxDateTime(endDate));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/calendarauto-function-dax
         */
        public DaxDateTime CalendarAuto(DaxInt fiscalYearEndMonth)
        {
            return new DaxDateTime($"CALENDARAUTO({fiscalYearEndMonth})");
        }

        public DaxDateTime CalendarAuto(string fiscalYearEndMonth)
        {
            return CalendarAuto(new DaxInt(fiscalYearEndMonth));
        }

        public DaxDateTime CalendarAuto(int fiscalYearEndMonth)
        {
            return CalendarAuto(new DaxInt(fiscalYearEndMonth));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/date-function-dax
         */
        public DaxDateTime Date(DaxInt year, DaxInt month, DaxInt day)
        {
            return new DaxDateTime($"DATE({year},{month},{day})");
        }

        public DaxDateTime Date(string year, string month, string day)
        {
            return Date(new DaxInt(year), new DaxInt(month), new DaxInt(day));
        }

        public DaxDateTime Date(int year, int month, int day)
        {
            return Date(year, month, day);
        }

        public enum DateDiffDaxInterval
        {
            Second,
            Seconde,
            Minute,
            Minuut,
            Hour,
            Uur,
            Day,
            Dag,
            Week,
            Month,
            Maand,
            Quarter,
            Kwartaal,
            Year,
            Jaar
        }

        private string DdtGet(DateDiffDaxInterval dateDiffDaxInterval)
        {
            switch (dateDiffDaxInterval)
            {
                case DateDiffDaxInterval.Dag:
                case DateDiffDaxInterval.Day:
                    return "DAY";
                case DateDiffDaxInterval.Uur:
                case DateDiffDaxInterval.Hour:
                    return "HOUR";
                case DateDiffDaxInterval.Seconde:
                case DateDiffDaxInterval.Second:
                    return "SECOND";
                case DateDiffDaxInterval.Minuut:
                case DateDiffDaxInterval.Minute:
                    return "MINUTE";
                case DateDiffDaxInterval.Week:
                    return "WEEK";
                case DateDiffDaxInterval.Maand:
                case DateDiffDaxInterval.Month:
                    return "MONTH";
                case DateDiffDaxInterval.Kwartaal:
                case DateDiffDaxInterval.Quarter:
                    return "QUARTER";
                case DateDiffDaxInterval.Jaar:
                case DateDiffDaxInterval.Year:
                    return "YEAR";
            }

            return "DAY";
        }

        /*
         * https://learn.microsoft.com/en-us/dax/datediff-function-dax
         */
        public DaxInt DateDiff(DaxDateTime date1, DaxDateTime date2, DateDiffDaxInterval dateDiffDaxInterval)
        {
            return new DaxInt($"DATEDIFF({date1},{date2},{DdtGet(dateDiffDaxInterval)})");
        }

        public DaxInt DateDiff(string date1, string date2, DateDiffDaxInterval dateDiffDaxInterval)
        {
            return DateDiff(new DaxDateTime(date1), new DaxDateTime(date2), dateDiffDaxInterval);
        }

        /*
         * https://learn.microsoft.com/en-us/dax/datevalue-function-dax
         */
        public DaxDateTime TextToDate(DaxString dateText)
        {
            return new DaxDateTime($"DATEVALUE({dateText})");
        }

        public DaxDateTime TextToDate(string dateText)
        {
            return TextToDate(new DaxString(dateText));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/day-function-dax
         */
        public DaxInt Day(DaxDateTime date)
        {
            return new DaxInt($"DAY({date})");
        }

        public DaxInt Day(DaxString date)
        {
            return new DaxInt($"DAY({date})");
        }

        public DaxInt Day(string date)
        {
            return Day(new DaxDateTime(date));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/edate-function-dax
         */
        public DaxDateTime EDate(DaxDateTime startDate, DaxInt addedMonths)
        {
            return new DaxDateTime($"EDATE({startDate},{addedMonths})");
        }

        public DaxDateTime EDate(string startDate, int addedMonths)
        {
            return EDate(new DaxDateTime(startDate), new DaxInt(addedMonths));
        }

        /*
         * https://learn.microsoft.com/en-us/dax/edate-function-dax
         */
        public DaxDateTime EDate(DaxString startDate, DaxInt addedMonths)
        {
            return new DaxDateTime($"EDATE({startDate},{addedMonths})");
        }

        public DaxDateTime EndOfMonth(DaxString startDate, DaxInt addedMonths)
        {
            return new DaxDateTime($"EOMONTH({startDate},{addedMonths})");
        }

        public DaxDateTime EndOfMonth(DaxDateTime startDate, DaxInt addedMonths)
        {
            return new DaxDateTime($"EOMONTH({startDate},{addedMonths})");
        }

        public DaxDateTime EndOfMonth(string startDate, int addedMonths)
        {
            return EndOfMonth(new DaxDateTime(startDate), new DaxInt(addedMonths));
        }


        public DaxInt Hour(DaxDateTime dateTime)
        {
            return new DaxInt($"HOUR({dateTime})");
        }

        public DaxInt Hour(string dateTime)
        {
            return Hour(new DaxDateTime(dateTime));
        }

        public DaxInt Minute(DaxDateTime dateTime)
        {
            return new DaxInt($"MINUTE({dateTime})");
        }

        public DaxInt Minute(string dateTime)
        {
            return Minute(new DaxDateTime(dateTime));
        }

        public enum DaxWeekend
        {
            SaturdaySunday = 1,
            SundayMonday = 2,
            MondayTuesday = 3,
            TuesdayWednesday = 4,
            WednesdayThursday = 5,
            ThursdayFriday = 6,
            FridaySaturday = 7,
            SundayOnly = 11,
            MondayOnly = 12,
            TuesdayOnly = 13,
            WednesdayOnly = 14,
            ThursdayOnly = 15,
            FridayOnly = 16,
            SaturdayOnly = 17
        }

        public DaxInt NetworkDays(DaxDateTime startDate, DaxDateTime endDate,
            DaxWeekend daxWeekend = DaxWeekend.SaturdaySunday)
        {
            return new DaxInt($"NETWORKDAYS({startDate},{endDate},{daxWeekend})");
        }

        public DaxInt NetworkDays(string startDate, string endDate,
            DaxWeekend daxWeekend = DaxWeekend.SaturdaySunday)
        {
            return NetworkDays(new DaxDateTime(startDate), new DaxDateTime(endDate), daxWeekend);
        }

        public DaxDateTime Now()
        {
            return new DaxDateTime("NOW()");
        }

        public DaxInt Quarter(DaxDateTime quarter)
        {
            return new DaxInt($"QUARTER({quarter})");
        }

        public DaxInt Quarter(string quarter)
        {
            return Quarter(new DaxDateTime(quarter));
        }

        public DaxInt Second(DaxDateTime second)
        {
            return new DaxInt($"SECOND({second})");
        }

        public DaxInt Second(string second)
        {
            return Second(new DaxDateTime(second));
        }

        public DaxDateTime Time(DaxInt hour, DaxInt minute, DaxInt second)
        {
            return new DaxDateTime($"TIME({hour},{minute},{second})");
        }

        public DaxDateTime Time(int hour, int minute, int second)
        {
            return Time(new DaxInt(hour), new DaxInt(minute), new DaxInt(second));
        }

        public DaxDateTime TimeValue(DaxString timeText)
        {
            return new DaxDateTime($"TIMEVALUE({timeText})");
        }

        public DaxDateTime TimeValue(string timeText)
        {
            return TimeValue(new DaxString(timeText));
        }

        public DaxDateTime Today()
        {
            return new DaxDateTime($"TODAY()");
        }

        public DaxDateTime UTCNow()
        {
            return new DaxDateTime($"UTCNOW()");
        }

        public DaxDateTime UTCToday()
        {
            return new DaxDateTime("UTCTODAY()");
        }

        public DaxInt WeekDay(DaxDateTime date, DaxInt returnType)
        {
            return new DaxInt($"WEEKDAY({date},{returnType})");
        }

        public DaxInt WeekDay(string date, int returnType)
        {
            return WeekDay(new DaxDateTime(date), new DaxInt(returnType));
        }

        public DaxInt WeekNum(DaxDateTime date, DaxInt returnType)
        {
            return new DaxInt($"WEEKNUM({date},{returnType})");
        }

        public DaxInt WeekNum(string date, int returnType)
        {
            return WeekNum(new DaxDateTime(date), new DaxInt(returnType));
        }

        public DaxInt Year(DaxDateTime date)
        {
            return new DaxInt($"YEAR({date})");
        }

        public DaxInt Year(string date)
        {
            return Year(new DaxDateTime(date));
        }

        public DaxFloat YearFrac(DaxDateTime startDate, DaxDateTime endDate, DaxInt basis)
        {
            return new DaxFloat($"YEARFRAC({startDate},{endDate},{basis})");
        }

        public DaxFloat YearFrac(string startDate, string endDate, int basis = 0)
        {
            return YearFrac(new DaxDateTime(startDate), new DaxDateTime(endDate), new DaxInt(basis));
        }

        #endregion

        #region FilterFunctions

        public DaxTable All(DaxTable table)
        {
            return new DaxTable($"ALL({table})");
        }

        public DaxTable All(string table)
        {
            return All(new DaxTable(table));
        }

        public DaxColumnArray All(DaxColumnArray columns)
        {
            return new DaxColumnArray($"ALL({columns})");
        }

        public DaxColumnArray All(params string[] columns)
        {
            return All(new DaxColumnArray(columns));
        }

        public DaxVoid AllCrossFiltered(DaxTable table)
        {
            return new DaxVoid($"ALLCROSSFILTERED({table})");
        }

        public DaxVoid AllCrossFiltered(string table)
        {
            return AllCrossFiltered(new DaxTable(table));
        }

        public DaxTable AllExcept(DaxTable table, DaxColumnArray columns)
        {
            return new DaxTable($"ALLEXCEPT({table},{columns}");
        }

        public DaxTable AllExcept(string table, params string[] columns)
        {
            return AllExcept(new DaxTable(table), new DaxColumnArray(columns));
        }

        public DaxTable AllNoBlankRow(DaxTable table)
        {
            return new DaxTable($"ALLNOBLANKROW({table})");
        }

        public DaxTable AllNoBlankRow(string table)
        {
            return AllNoBlankRow(new DaxTable(table));
        }

        public DaxColumnArray AllNoBlankRow(DaxColumnArray columns)
        {
            return new DaxColumnArray($"ALLNOBLANKROW({columns})");
        }

        public DaxColumnArray AllNoBlankRow(params string[] columns)
        {
            return AllNoBlankRow(new DaxColumnArray(columns));
        }

        public DaxTable AllSelected(DaxTable table)
        {
            return new DaxTable($"ALLSELECTED({table})");
        }

        public DaxTable AllSelected(string table)
        {
            return AllSelected(new DaxTable(table));
        }

        public DaxColumnArray AllSelected(DaxColumnArray columns)
        {
            return new DaxColumnArray($"ALLSELECTED({columns})");
        }

        public DaxColumnArray AllSelected(params string[] columns)
        {
            return AllSelected(new DaxColumnArray(columns));
        }

        public DaxString Calculate(DaxExpression expression)
        {
            return new DaxString($"CALCULATE({expression})");
        }

        public DaxString Calculate(string expression)
        {
            return Calculate(new DaxExpression(expression));
        }

        public DaxString Calculate(DaxExpression expression, DaxFilterArray filters)
        {
            return new DaxString($"CALCULATE({expression},{filters}");
        }

        public DaxString Calculate(string expression, params string[] filters)
        {
            return Calculate(new DaxExpression(expression), new DaxFilterArray(filters));
        }

        public DaxString CalculateTable(DaxExpression expression)
        {
            return new DaxString($"CALCULATETABLE({expression})");
        }

        public DaxString CalculateTable(string expression)
        {
            return Calculate(new DaxExpression(expression));
        }

        public DaxString CalculateTable(DaxExpression expression, DaxFilterArray filters)
        {
            return new DaxString($"CALCULATETABLE({expression},{filters}");
        }

        public DaxString CalculateTable(string expression, params string[] filters)
        {
            return CalculateTable(new DaxExpression(expression), new DaxFilterArray(filters));
        }

        public DaxString Earlier(DaxColumn column, DaxInt number)
        {
            return new DaxString($"EARLIER({column},{number})");
        }

        public DaxString Earlier(string column, int number)
        {
            return Earlier(new DaxColumn(column), new DaxInt(number));
        }

        public DaxColumn Earliest(DaxColumn column)
        {
            return new DaxColumn($"EARLIEST({column})");
        }

        public DaxColumn Earliest(string column)
        {
            return Earliest(new DaxColumn(column));
        }

        public DaxTable Filter(DaxTable table, DaxFilter filter)
        {
            return new DaxTable($"FILTER({table},{filter})");
        }

        public DaxTable Filter(string table, string filter)
        {
            return Filter(new DaxTable(table), new DaxFilter(filter));
        }

        public DaxTable KeepFilters(DaxExpression expression)
        {
            return new DaxTable($"KEEPFILTERS({expression})");
        }

        public DaxTable KeepFilters(string expression)
        {
            return KeepFilters(new DaxExpression(expression));
        }

        public DaxString LookupValue(DaxString resultColumnName, params Tuple<DaxString, DaxString>[] searches)
        {
            var sb = new StringBuilder();
            sb.Append($"LOOKUPVALUE({resultColumnName},");
            foreach (var search in searches)
            {
                sb.Append(search.Item1);
                sb.Append(',');
                sb.Append(search.Item2);
                sb.Append(',');
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(')');
            return new DaxString(sb.ToString());
        }

        public DaxString LookupValue(string resultColumnName, params Tuple<DaxString, DaxString>[] searches)
        {
            return LookupValue(new DaxString(resultColumnName), searches);
        }

        public DaxVoid RemoveFilters(DaxTable table)
        {
            return new DaxVoid($"REMOVEFILTERS({table})");
        }

        public DaxVoid RemoveFilters(string table)
        {
            return RemoveFilters(new DaxTable(table));
        }

        public DaxVoid RemoveFilters(DaxColumnArray columns)
        {
            return new DaxVoid($"REMOVEFILTERS({columns})");
        }

        public DaxVoid RemoveFilters(params string[] columns)
        {
            return RemoveFilters(new DaxColumnArray(columns));
        }

        public DaxString SelectedValue(DaxString columnName)
        {
            return new DaxString($"SELECTEDVALUE({columnName})");
        }

        public DaxString SelectedValue(string columnName)
        {
            return SelectedValue(new DaxString(columnName));
        }

        public DaxString SelectedValue(DaxString columnName, DaxString alternateResult)
        {
            return new DaxString($"SELECTEDVALUE({columnName},{alternateResult})");
        }

        public DaxString SelectedValue(string columnName, string alternateResult)
        {
            return SelectedValue(new DaxString(columnName), new DaxString(alternateResult));
        }

        #endregion

        #region FinancialFunctions

        public DaxDouble AccruedInterest(DaxDateTime issue, DaxDateTime firstInterest, DaxDateTime settlement,
            DaxDouble rate, DaxDouble par, DaxInt frequency, DaxInt basis, DaxBool calcMethod)
        {
            return new DaxDouble(
                $"ACCRINT({issue},{firstInterest},{settlement},{rate},{par},{frequency},{basis},{calcMethod})");
        }

        public DaxDouble AccruedInterest(string issue, string firstInterest, string settlement, double rate, double par,
            int frequency, int basis = 0, bool calcMethod = true)
        {
            return AccruedInterest(new DaxDateTime(issue), new DaxDateTime(firstInterest), new DaxDateTime(settlement),
                new DaxDouble(rate), new DaxDouble(par), new DaxInt(frequency), new DaxInt(basis),
                new DaxBool(calcMethod));
        }

        public DaxDouble AccruedInterestMaturity(DaxDateTime issue, DaxDateTime maturity, DaxDouble rate, DaxDouble par,
            DaxInt basis)
        {
            return new DaxDouble($"ACCRINTM({issue},{maturity},{rate},{par},{basis})");
        }

        public DaxDouble AccruedInterestMaturity(string issue, string maturity, double rate, double par, int basis = 0)
        {
            return AccruedInterestMaturity(new DaxDateTime(issue), new DaxDateTime(maturity), new DaxDouble(rate),
                new DaxDouble(par), new DaxInt(basis));
        }

        public DaxDouble DeprecationPerAccountingPeriod(DaxInt cost, DaxDateTime datePurchased, DaxDateTime firstPeriod,
            DaxInt salvage, DaxInt period, DaxDouble rate, DaxInt basis)
        {
            return new DaxDouble(
                $"AMORDEGRC({cost}, {datePurchased}, {firstPeriod}, {salvage}, {period}, {rate},{basis}");
        }

        public DaxDouble DeprecationPerAccountingPeriod(int cost, string datePurchased, string firstPeriod, int salvage,
            int period, double rate, int basis = 0)
        {
            return new DaxDouble
                ($"AMORDEGRC({cost}, {datePurchased}, {firstPeriod}, {salvage}, {period}, {rate},{basis}");
        }

        #endregion

        #region InformationFunctions

        #endregion

        #region LogicalFunctions

        #endregion

        #region MathAndTrigFunctions

        #endregion

        #region OtherFunctions

        #endregion

        #region ParentAndChildFunctions

        #endregion

        #region RelationshipFunctions

        #endregion

        #region StatisticalFunctions

        #endregion

        #region TableManipulationFunctions

        #endregion

        #region TextFunctions

        #endregion

        #region TimeDaxIntelligenceFunctions

        #endregion
    }
}