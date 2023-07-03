using Cron.NET.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Cron.NET
{
    public class CronExpression
    {
        public static int MaxIterationCount { get; set; } = 1000;

        public MinuteField Minute { get; set; }
        public HourField Hour { get; set; }
        public DayOfMonthField DayOfMonth { get; set; }
        public MonthField Month { get; set; }
        public DayOfWeekField DayOfWeek { get; set; }

        public string Literal { get; set; }
        public CronExpression(string literal)
        {
            if (CronExpression.Literals.ContainsKey(literal))
            {
                this.Literal = CronExpression.Literals[literal];
            }
            else
            {
                this.Literal = literal;
            }

            var chunks = this.Literal.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            this.Minute = new MinuteField(chunks[0]);
            this.Hour = new HourField(chunks[1]);
            this.DayOfMonth = new DayOfMonthField(chunks[2]);
            this.Month = new MonthField(chunks[3]);
            this.DayOfWeek = new DayOfWeekField(chunks[4]);
        }

        public static Dictionary<string, string> Literals { get; set; } = new Dictionary<string, string>()
        {
            {"@yearly", "0 0 1 1 *" },
            {"@annually", "0 0 1 1 *" },
            {"@monthly", "0 0 1 * *" },
            {"@weekly", "0 0 * * 0" },
            {"@daily", "0 0 * * *" },
            {"@hourly", "0 * * * *" },
        };

        public DateTime GetNextDate(DateTime currentDate, int skip = 0, bool allowCurrentDate = false)
        {
            return this.GetRunDate(currentDate, skip, allowCurrentDate, false);
        }

        public List<DateTime> GetNextDates(DateTime currentDate, int amount, bool allowCurrentDate = false)
        {
            List<DateTime> matches = new List<DateTime>();
            for (int i = 0; i < amount; i++)
            {
                matches.Add(this.GetRunDate(currentDate, i, allowCurrentDate, false));
            }

            return matches;
        }

        public DateTime GetPreviousDate(DateTime currentDate, int skip = 0, bool allowCurrentDate = false)
        {
            return this.GetRunDate(currentDate, skip, allowCurrentDate, true);
        }

        public List<DateTime> GetPreviousDates(DateTime currentDate, int amount, bool allowCurrentDate = false)
        {
            List<DateTime> matches = new List<DateTime>();
            for (int i = 0; i < amount; i++)
            {
                matches.Add(this.GetRunDate(currentDate, i, allowCurrentDate, true));
            }

            return matches;
        }


        private DateTime GetSatisfying(CronField field, DateTime currentDate, bool decrement = false)
        {
            DateTime nextRun = currentDate;
            for (int i = 0; i < CronExpression.MaxIterationCount; i++)
            {
                if (!field.Satisfies(nextRun))
                {
                    nextRun = field.Increment(nextRun, decrement);
                    continue;
                }

                return nextRun;
            }

            throw new Exception();
        }

        private DateTime GetRunDate(DateTime currentDate, int skip, bool allowCurrentDate, bool decrement = false)
        {
            DateTime nextRun = currentDate;

            List<List<CronField>> fieldOrder = new List<List<CronField>>
            {
                new List<CronField>{this.Month },
                new List<CronField>{this.DayOfMonth, this.DayOfWeek },
                new List<CronField>{this.Hour },
                new List<CronField>{this.Minute },
            };

            List<List<CronField>> executionOrder = new List<List<CronField>>();
            foreach (var fieldPlane in fieldOrder)
            {
                List<CronField> plane = new List<CronField>();
                foreach (var field in fieldPlane)
                {
                    if (!field.AnyValue)
                    {
                        plane.Add(field);
                    }
                }

                if (plane.Count() > 0)
                {
                    executionOrder.Add(plane);
                }
            }

            for (int i = 0; i < CronExpression.MaxIterationCount; i++)
            {
                bool satisfied = false;
                foreach (var plane in executionOrder)
                {
                    satisfied = false;
                    List<DateTime> satisfyingDates = new List<DateTime>();
                    foreach (var part in plane)
                    {
                        if (part.Satisfies(nextRun))
                        {
                            satisfyingDates.Add(nextRun);
                            satisfied = true;
                            break;
                        }
                    }

                    if (!satisfied)
                    {
                        foreach (var part in plane)
                        {
                            satisfyingDates.Add(this.GetSatisfying(part, nextRun, decrement));
                        }
                    }

                    if (satisfyingDates.Any())
                    {
                        if (decrement)
                        {
                            nextRun = satisfyingDates.OrderByDescending(x => x).First();
                        }
                        else
                        {
                            nextRun = satisfyingDates.OrderBy(x => x).First();
                        }
                    }
                }

                if (!satisfied)
                {
                    continue;
                }

                if ((!allowCurrentDate && nextRun == currentDate) || --skip > -1)
                {
                    this.Minute.Increment(nextRun, decrement);
                    continue;
                }

                return nextRun;
            }

            throw new Exception();
        }
    }
}
