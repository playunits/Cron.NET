namespace Cron.NET.Fields
{
    public class MinuteField : CronField
    {
        public MinuteField(string literal) : base(literal, Constraint.Minute)
        {
        }

        public override bool Satisfies(DateTime date)
        {
            return base.Satisfies(date) || this.Values.Contains(date.Minute);
        }
        public override DateTime Increment(DateTime date, bool decrement = false)
        {
            //return date.AddMinutes(decrement ? -1 : 1);
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, this.IncrementInternal(date.Minute, decrement), 0);
        }
    }
}
