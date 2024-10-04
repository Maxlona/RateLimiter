using NUnit.Framework;
namespace RateLimiter.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RateLimiter.Configs;
using RateLimiter.Rules;
using RateLimiter.Rules.SlidingWindow;
using RateLimiter.Storage;
using RateLimiter.Utilities;
using System;

[TestFixture]
public class RateLimiterEngine_Test
{
    public ServiceProvider Set()
    {
        IServiceCollection services = new ServiceCollection();
        IRateLimiterConfigs configs = Substitute.For<IRateLimiterConfigs>();
        configs.BindConfig().Returns(new Models.ConfigValues()
        {
            Enabled = true,
            MaxAllowed = 20,    // 20 calls max
            PerSecondsTimeFrame = 5       // each 5 seconds
        });

        IDateTimeService datetime = Substitute.For<IDateTimeService>();
        datetime.GetCurrentTime().Returns(DateTime.Now);
        services.AddSingleton<IRateLimiterConfigs, RateLimiterConfigs>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddSingleton<IStorage, Storage>();
        services.AddSingleton<IRulesEvaluator, RulesEvaluator>();
        return services.BuildServiceProvider();
    }

    [Test]
    public void check_dependency_before_init()
    {
        var serviceProvider = Set();
        var Limiter = serviceProvider?.GetService<IRulesEvaluator>();
        var storage = serviceProvider?.GetService<IStorage>();
        Assert.IsNotNull(Limiter);
        Assert.IsInstanceOfType<IRulesEvaluator>(Limiter);
    }

    /// <summary>
    /// test rules evaluator
    /// </summary>
    /// <param name="start"></param>
    /// <param name="last"></param>
    /// <param name="callsCount"></param>
    /// <param name="shouldPass"></param>
    //[TestCase("8/5/2024 10:25:01 PM", "8/5/2024 10:25:02 PM", 100, false, Description = "100 calls per 1 second")]
    //[TestCase("8/5/2024 10:25:01 PM", "8/5/2024 10:25:06 PM", 15, true, Description = "15 calls per 5 seconds")]
    //[TestCase("8/5/2024 10:25:01 PM", "8/5/2024 10:25:06 PM", 25, false, Description = "25 call per 5 seconds")]
    //[TestCase("8/5/2024 10:25:01 PM", "8/5/2024 10:25:21 PM", 10, true, Description = "10 call per 20 seconds")]
    //[TestCase("8/5/2024 10:25:01 PM", "8/5/2024 10:25:06 PM", 20, true, Description = "default: 20 call per 5 seconds")]
    //public void Check_Can_Access_rules(DateTime start, DateTime last, int callsCount, bool shouldPass)
    //{
    //    var serviceProvider = Set();

    //    var configs = Substitute.For<IRateLimiterConfigs>();
    //    configs.BindConfig().Returns(new Models.ConfigValues() { Enabled = true, MaxAllowed = 20, PerSecondsTimeFrame = 5 });

    //    var date = Substitute.For<IDateTimeService>();
    //    date.GetCurrentTime().Returns(DateTime.Now);   

    //    var Store = new Storage(configs, date);

    //    var storage = serviceProvider.GetService<IStorage>();
    //    var rules = serviceProvider.GetService<IRulesEvaluator>();
    //    rules = new RulesEvaluator();
    //    var Limiter = rules.Limiter(new SlidingWindow(configs, storage));

    //    bool? CanAccess = Limiter?.CanAccess(start, last, callsCount);
    //    Assert.IsTrue(shouldPass == CanAccess);
    //}
}