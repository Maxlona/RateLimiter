using NUnit.Framework;
namespace RateLimiter.Tests;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RateLimiter.Configs;
using RateLimiter.Rules;
using RateLimiter.Rules.SlidingWindow;
using RateLimiter.Storage;
using RateLimiter.Utilities;
using System;

[TestFixture]
public class QueryTraffic_Test
{
    // session = new user
    Guid session1;

    /// mock config and datetime service
    ServiceProvider serviceProvider;
    IRateLimiterConfigs configs = Substitute.For<IRateLimiterConfigs>();
    IStorage store = Substitute.For<IStorage>();
    IDateTimeService time = Substitute.For<IDateTimeService>();

    [SetUp]
    public void Init()
    {
        IServiceCollection services = new ServiceCollection();
        time.GetCurrentTime().Returns(DateTime.Now);
        services.AddSingleton<IRateLimiterConfigs, RateLimiterConfigs>();
        services.AddSingleton<IStorage, Storage>();
        serviceProvider = services.BuildServiceProvider();
        session1 = Guid.NewGuid();
        store = new Storage(configs, time);
    }


    [Test]
    public void Check_Rate_limiter_storage_query()
    {
        // mock config service
        configs.BindConfig().Returns(new Models.ConfigValues()
        {
            Enabled = true,
            MaxAllowed = 20,    // 20 calls max
            PerSecondsTimeFrame = 5       // each 5 seconds
        });

        var rules = new RulesEvaluator();
        var limiter = rules.Limiter(new SlidingWindow(configs, store));

        Random r = new Random();

        // add 100 visits per session into past 1/2 hr 
        for (int i = 0; i < 100; i++)
        {
            int randomHalfHour = r.Next(1, 30);
            randomHalfHour *= -1;
            /// add past 10 minutes time frame
            time.GetCurrentTime().Returns(DateTime.Now.AddMinutes(randomHalfHour));
            store.AddOrAppend(session1);
        }

        Assert.NotNull(store?.Get(session1));

        var totalRequests = store?.Get(session1);

        /// random added dates need to be sorted
        totalRequests?.Sort();

        /// since we are injecting random datetime,
        /// check before assert, if any datetime was within the limited per mock config range
        if (totalRequests?.Count > 1 && totalRequests?.Count < configs.BindConfig().MaxAllowed)  //max allowed per config-mock
        {

            bool? canAccess = limiter?.CanAccess(totalRequests);
            Assert.IsTrue(canAccess.Value == true);
        }
    }



    [Test]
    public void check_Limiter_Add_New_Sessions_and_Evaluate()
    {

        // mock config service
        // each 5 secs, 20 calls are allowed
        configs.BindConfig().Returns(new Models.ConfigValues()
        {
            Enabled = true,
            MaxAllowed = 20,
            PerSecondsTimeFrame = 5
        });

        var rules = new RulesEvaluator();
        var limiter = rules.Limiter(new SlidingWindow(configs, store));


        bool? eval = true;
        // add 100 visits per session into past 1/2 hr 
        for (int i = 0; i < 100; i++)
        {
            eval = limiter?.LogVisitAndEvaluate(session1);
        }

        /// should reject excessive calls
        Assert.IsFalse(eval);
    }

}
