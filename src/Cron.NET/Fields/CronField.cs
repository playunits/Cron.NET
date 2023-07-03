using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Cron.NET.Helpers;

namespace Cron.NET.Fields
{

    public class CronField
    {
        public string Literal { get; set; }

        public List<int> Values { get; set; }

        public bool AnyValue => this.Literal.Contains('*');


        //public bool HasRange => this.Literal.Contains('-');

        //public bool HasIncrement => this.Literal.Contains('/');

        //public bool IsAnyValue => this.Literal.StartsWith('*');

        //public List<int> FullRange => Enumerable.Range(this.Constraint.Min, this.Constraint.Max - this.Constraint.Min).ToList();

        public Constraint Constraint { get; set; }

        public CronField(string literal, Constraint constraint)
        {
            Literal = literal;
            Constraint = constraint;
            Values = Parse(ConvertLiterals(literal));
        }

        private List<int> Parse(string literal)
        {

            List<string> chunks = new List<string>() { literal };
            if (literal.Contains(','))
            {
                chunks = literal.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            }

            List<int> values = new List<int>();

            foreach (var chunk in chunks)
            {
                values.AddRange(ParseValue(chunk));
            }
            return values.Distinct().OrderBy(x => x).ToList();
        }

        private string ConvertLiterals(string input)
        {
            if (Constraint.Aliases is not null)
            {
                foreach (var kvp in Constraint.Aliases)
                {
                    input = input.Replace(kvp.Key, kvp.Value, StringComparison.OrdinalIgnoreCase);
                }
            }
            return input;
        }

        private List<int> ParseValue(string token)
        {
            int step = 1;
            string valueChunk = token;
            if (token.Contains('/'))
            {
                var chunks = token.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                if (chunks.Length == 2)
                {
                    valueChunk = chunks[0];
                    if (int.TryParse(chunks[1], out int iStep))
                    {
                        step = iStep;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }

            }

            if (valueChunk.Contains('-'))
            {
                var rangeChunks = valueChunk.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (rangeChunks.Length == 2)
                {
                    if (int.TryParse(rangeChunks[0], out int start) && int.TryParse(rangeChunks[1], out int end))
                    {
                        if (start < Constraint.Min || start > Constraint.Max || start > end)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        if (end < Constraint.Min || end > Constraint.Max || end < start)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        if (step > end - start + 1)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        return EnumerableExtensions.Range(start, end, step).ToList();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }

            }
            else
            {
                if (string.Equals(valueChunk, "*"))
                {
                    return EnumerableExtensions.Range(Constraint.Min, Constraint.Max).ToList();
                }
                else
                {
                    if (int.TryParse(valueChunk, out int value))
                    {
                        return new List<int>() { value };
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public virtual bool Satisfies(DateTime date)
        {
            return this.AnyValue;
        }

        public virtual DateTime Increment(DateTime date, bool decrement = false)
        {
            return date;
        }

        protected int IncrementInternal(int value, bool decrement = false)
        {
            if (decrement)
            {
                if (value == this.Constraint.Min)
                {
                    value = this.Constraint.Max;
                }
                else
                {
                    value -= 1;
                }
            }
            else
            {
                if (value == this.Constraint.Max)
                {
                    value = this.Constraint.Min;
                }
                else
                {
                    value += 1;
                }
            }

            return value;
        }
    }
}
