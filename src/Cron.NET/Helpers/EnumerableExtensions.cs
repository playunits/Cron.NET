namespace Cron.NET.Helpers
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<int> Range(int start, int end, int step = 1)
        {
            List<int> result = new List<int>();
            for (int i = start; i <= end; i += step)
            {
                result.Add(i);
            }

            return result.Cast<int>();
        }
    }
}
