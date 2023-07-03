namespace Cron.NET.Fields
{
    public class HourField : CronField
    {
        public HourField(string literal) : base(literal, Constraint.Hour)
        {
        }

        public override bool Satisfies(DateTime date)
        {
            return base.Satisfies(date) || this.Values.Contains(date.Hour);
        }

        public override DateTime Increment(DateTime date, bool decrement = false)
        {
            return date.AddHours(decrement ? -1 : 1);
            //return new DateTime(date.Year, date.Month, date.Day, this.IncrementInternal(date.Hour, decrement), date.Minute, 0);
        }
    }
}
