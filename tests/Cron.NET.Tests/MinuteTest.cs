namespace Cron.NET.Tests;

public class MinuteTest
{
    [Fact]
    public void AllMinutes()
    {
        CronJob c = CronJob.Parse("* * * * *");

        Assert.Equal(60, c.Minutes.Count);
    }

    [Fact]
    public void AllMinutesStep()
    {
        CronJob c = CronJob.Parse("*/2 * * * *");

        Assert.Equal(30, c.Minutes.Count);        
    }


    [Fact]
    public void MinuteList()
    {
        CronJob c = CronJob.Parse("1,5,7,32,40,12 * * * *");

        Assert.Equal(6, c.Minutes.Count);
        Assert.Collection(c.Minutes,
            item => Assert.Equal(1, item),
            item => Assert.Equal(5, item),
            item => Assert.Equal(7, item),
            item => Assert.Equal(32, item),
            item => Assert.Equal(40, item),
            item => Assert.Equal(12, item)
        );
    }

    [Fact]
    public void MinuteRange()
    {
        CronJob c = CronJob.Parse("1-15 * * * *");

        Assert.Equal(15, c.Minutes.Count);
    }

    [Fact]
    public void MinuteRangeStep()
    {
        CronJob c = CronJob.Parse("1-15/2 * * * *");

        Assert.Equal(8, c.Minutes.Count);
        foreach(int i in c.Minutes)
        {
            Assert.True(i % 2 == 1);
        }
    }

}