using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cron.NET
{
    public static class Parser
    {

        // Possible Inputs:

        // * - Step allowed
        // 1
        // 1-2 - step allowed        
        // 1,2,3

        private static int ParseSingle(string element, Constraint constraint)
        {
            element = constraint.Aliases?[element.ToLower()] ?? element;

            if (Int32.TryParse(element, out int parsedElement))
            {

                if (parsedElement < constraint.Min || parsedElement > constraint.Max)
                {
                    throw new Exception("Element out of bounds");
                }

                return parsedElement;
            }
            else
            {
                throw new Exception("Element is NaN");
            }
        }

        private static List<int> ParseStarValue(string element, Constraint constraint)
        {
            List<int> result = new List<int>();

            Regex rx = new Regex(@"([*A-Za-z0-9]+)(?:/([0-9]+))?");
            Match match = rx.Match(element);

            int step = 1;
            if (!String.IsNullOrWhiteSpace(match.Groups[2].Value))
            {
                if (Int32.TryParse(match.Groups[2].Value, out int tStep))
                {
                    step = tStep;
                }
                else
                {
                    throw new Exception("The Step Value needs to be a Number");
                }
            }

            for (int i = constraint.Min; i <= constraint.Max; i += step)
            {
                result.Add(i);
            }

            return result;

        }

        private static List<int> ParseListValue(string element, Constraint constraint)
        {
            List<int> result = new List<int>();

            List<string> listElements = element.Split(',').ToList();
            if (listElements.Count > 0)
            {
                foreach (string listElement in listElements)
                {
                    result.AddRange(ParseElement(listElement, constraint));
                }
            }
            return result;

        }

        private static List<int> ParseRangeValue(string element, Constraint constraint)
        {
            List<int> result = new List<int>();

            Regex rx = new Regex(@"([A-Za-z0-9]+)-([A-Za-z0-9]+)(?:/([0-9]+))?");
            Match match = rx.Match(element);

            int start = ParseSingle(match.Groups[1].Value, constraint);
            int end = ParseSingle(match.Groups[2].Value, constraint);

            if (end < start)
            {
                throw new Exception("End must be greater than start");
            }


            int step = 1;
            if (!String.IsNullOrWhiteSpace(match.Groups[3].Value))
            {
                if (Int32.TryParse(match.Groups[3].Value, out int tStep))
                {
                    step = tStep;
                }
                else
                {
                    throw new Exception("The Step Value needs to be a Number");
                }
            }

            for (int i = start; i <= end; i += step)
            {
                result.Add(i);
            }

            return result;
        }

        public static List<int> ParseElement(string element, Constraint constraint)
        {
            // Handle Stars
            if (element.StartsWith('*'))
            {
                return ParseStarValue(element, constraint);
            }

            // Handle Lists

            if (element.Contains(','))
            {
                return ParseListValue(element, constraint);
            }



            Regex rx = new Regex(@"([A-Za-z0-9]+)-([A-Za-z0-9]+)(?:/([0-9]+))?");
            if (rx.IsMatch(element))
            {
                return ParseRangeValue(element, constraint);
            }
            else
            {
                return new List<int> { ParseSingle(element, constraint) };
            }


        }
    }
}