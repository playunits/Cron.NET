namespace Cron.NET.Fields
{
    public class DayOfMonthField : CronField
    {
        public DayOfMonthField(string literal) : base(literal, Constraint.Day)
        {
        }

        public override bool Satisfies(DateTime date)
        {
            return base.Satisfies(date) || this.Values.Contains(date.Day);
        }
        public override DateTime Increment(DateTime date, bool decrement = false)
        {
            return date.AddDays(decrement ? -1 : 1);
        }
    }
}
