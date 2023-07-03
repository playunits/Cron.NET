namespace Cron.NET.Fields
{
    public class MonthField : CronField
    {
        public MonthField(string literal) : base(literal, Constraint.Month)
        {
        }

        public override bool Satisfies(DateTime date)
        {
            return base.Satisfies(date) || this.Values.Contains(date.Month);
        }
        public override DateTime Increment(DateTime date, bool decrement = false)
        {
            return date.AddMonths(decrement ? -1 : 1);
            //return new DateTime(date.Year, this.IncrementInternal(date.Month, decrement), date.Day, date.Hour, date.Minute, 0);
        }
    }
}
