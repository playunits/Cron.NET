namespace Cron.NET.Tests;

public class PredictionTest
{
    public static DateTime Start { get; set; } = new DateTime(2022, 6, 5, 21, 29, 43);

    [Fact]
    public void TestForwardOne()
    {
        CronJob c = CronJob.Parse("* 2 1 * WED");

        DateTime? next = c.GetNext(Start);

        Assert.Equal(new DateTime(2022, 6, 8, 2, 0, 0),next);
    }

    [Fact]
    public void TestForwardFive()
    {
        CronJob c = CronJob.Parse("* 2 1 * WED");

        List<DateTime> nexts = c.GetNext(5,DateTime.Now);

        Assert.Collection(nexts,
            item => Assert.Equal(new DateTime(2022, 6, 8, 2, 0, 0), item),
            item => Assert.Equal(new DateTime(2022, 6, 15, 2, 0, 0), item),
            item => Assert.Equal(new DateTime(2022, 6, 22, 2, 0, 0), item),
            item => Assert.Equal(new DateTime(2022, 6, 29, 2, 0, 0), item),
            item => Assert.Equal(new DateTime(2022, 7, 1, 2, 0, 0), item)
        );
    }

    //[Fact]
    //public void TestBackwardOne()
    //{
    //    CronJob c = CronJob.Parse("* 2 1 * WED");

    //    DateTime? next = c.GetLast(Start);

    //    Assert.Equal(new DateTime(2022, 6, 1, 2, 0, 0), next);
    //}

    //[Fact]
    //public void TestBackwardFive()
    //{
    //    CronJob c = CronJob.Parse("* 2 1 * WED");

    //    List<DateTime> nexts = c.GetLast(5, DateTime.Now);

    //    Assert.Collection(nexts,
    //        item => Assert.Equal(new DateTime(2022, 6, 1, 2, 0, 0), item),
    //        item => Assert.Equal(new DateTime(2022, 5, 25, 2, 0, 0), item),
    //        item => Assert.Equal(new DateTime(2022, 5, 18, 2, 0, 0), item),
    //        item => Assert.Equal(new DateTime(2022, 5, 11, 2, 0, 0), item),
    //        item => Assert.Equal(new DateTime(2022, 5, 4, 2, 0, 0), item)
    //    );
    //}

}