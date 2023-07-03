using Cron.NET;

namespace Cron.Net.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exp = new CronExpression("0 0 1-31 2 Mon");
            var date = exp.GetNextDate(DateTime.Now);

            Console.WriteLine(date);
        }
    }
}