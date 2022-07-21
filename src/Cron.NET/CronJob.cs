using System;
using System.Collections.Generic;
using System.Linq;

namespace Cron.NET
{
    public class CronJob
    {
        public List<int> Seconds { get; set; } = new List<int>();
        public List<int> Minutes { get; set; } = new List<int>();
        public List<int> Hours { get; set; } = new List<int>();
        public List<int> Days { get; set; } = new List<int>();
        public List<int> Months { get; set; } = new List<int>();
        public List<int> Weekdays { get; set; } = new List<int>();

        public static CronJob Parse(string cronString)
        {
            List<string> elements = cronString.Split(' ').ToList();
            if (elements.Count != 5)
            {
                throw new Exception("Invalid Cron");
            }

            return new CronJob()
            {
                Minutes = Parser.ParseElement(elements[0], Constraint.Minute),
                Hours = Parser.ParseElement(elements[1], Constraint.Hour),
                Days = Parser.ParseElement(elements[2], Constraint.Day),
                Months = Parser.ParseElement(elements[3], Constraint.Month),
                Weekdays = Parser.ParseElement(elements[4], Constraint.Weekday)
            };
        }

        public DateTime? GetNext(DateTime? start = null, DateTime? end = null)
        {
            start = start ?? DateTime.Now;
            end = end ?? DateTime.MaxValue;


            DateTime next = new 
            int leapYearCount = 0;
            // Accommodate for Leap Years
            while (leapYearCount < 5)
            {
                int tries = 0;
                // Max Try Limit = 12 Months + 31 Days + 24 Hours + 60 Minutes = 127 Meaning that it could not be found within this year
                while (tries < 127)
                {
                    // When this is already bigger than our end date, we wont find any better
                    if (next > end)
                    {
                        return null;
                    }

                    if (!Minutes.Contains(next.Minute))
                    {
                        int minute = next.Minute + 1;
                        int hour = next.Hour;

                        // Minute Rollover
                        if (minute > 60)
                        {
                            minute = 0;
                            hour += 1;
                        }


                        next = new DateTime(next.Year + leapYearCount, next.Month, next.Day, hour, minute, 0);
                        tries++;
                        continue;
                    }

                    if (!Hours.Contains(next.Hour))
                    {
                        int hour = next.Hour + 1;
                        int day = next.Day;

                        // Hour Rollover
                        if (hour > 23)
                        {
                            hour = 0;
                            day += 1;
                        }

                        next = new DateTime(next.Year + leapYearCount, next.Month, day, hour, next.Minute, 0);
                        tries++;
                        continue;
                    }

                    if (!Days.Contains(next.Day) && !Weekdays.Contains((int)next.DayOfWeek))
                    {
                        int daysInMonth = DateTime.DaysInMonth(next.Year, next.Month);
                        int day = next.Day + 1;
                        int month = next.Month;

                        //Day Rollover
                        if (day > daysInMonth)
                        {
                            day = 1;
                            month += 1;
                        }
                        next = new DateTime(next.Year + leapYearCount, month, day, next.Hour, next.Minute, 0);
                        tries++;
                        continue;
                    }

                    if (!Months.Contains(next.Month))
                    {
                        int month = next.Month + 1;

                        //Year Rollover
                        if (month > 12)
                        {
                            month = 1;
                            leapYearCount += 1;
                        }
                        next = new DateTime(next.Year + leapYearCount, month, next.Day, next.Hour, next.Minute, 0);
                        tries++;
                        continue;
                    }
                    

                    return next;
                }
                leapYearCount++;
            }

            return null;
        }

        public List<DateTime> GetNext(int count, DateTime? start = null, DateTime? end = null)
        {
            List<DateTime> results = new List<DateTime>();

            start = start ?? DateTime.Now;
            end = end ?? DateTime.MaxValue;

            for (int i = 0; i < count; i++)
            {
                DateTime? result = GetNext(start, end);
                if (result is not null)
                {
                    results.Add((DateTime)result);
                    start = result;
                }
                else
                {
                    break;
                }
            }

            return results;
        }
        
        //TODO: GetLast


    }
}