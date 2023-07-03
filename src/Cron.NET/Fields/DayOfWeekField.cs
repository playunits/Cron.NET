namespace Cron.NET.Fields
{
    public class DayOfWeekField : CronField
    {
        public DayOfWeekField(string literal) : base(literal, Constraint.Weekday)
        {
        }

        public override bool Satisfies(DateTime date)
        {
            return base.Satisfies(date) || this.Values.Contains((int)date.DayOfWeek);
        }

        public override DateTime Increment(DateTime date, bool decrement = false)
        {
            return date.AddDays(decrement ? -1 : 1);
        }
    }
}
