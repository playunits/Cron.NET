using System.Collections.Generic;

namespace Cron.NET
{
    public class Constraint
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public Dictionary<string, string>? Aliases { get; set; }

        public static Constraint Minute = new Constraint()
        {
            Min = 0,
            Max = 59
        };

        public static Constraint Hour = new Constraint()
        {
            Min = 0,
            Max = 23
        };

        public static Constraint Day = new Constraint()
        {
            Min = 1,
            Max = 31
        };

        public static Constraint Month = new Constraint()
        {
            Min = 1,
            Max = 12,
            Aliases = new Dictionary<string, string>(){
                {"jan","1"},
                {"feb","2"},
                {"mar","3"},
                {"apr","4"},
                {"may","5"},
                {"jun","6"},
                {"jul","7"},
                {"aug","8"},
                {"sep","9"},
                {"oct","10"},
                {"nov","11"},
                {"dec","12"}
            }
        };

        public static Constraint Weekday = new Constraint()
        {
            Min = 0,
            Max = 6,
            Aliases = new Dictionary<string, string>(){
                {"sun","0"},
                {"mon","1"},
                {"tue","2"},
                {"wed","3"},
                {"thu","4"},
                {"fri","5"},
                {"sat","6"}
            }
        };
    
    }
}