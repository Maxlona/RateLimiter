using Microsoft.Extensions.DependencyInjection;
using RateLimiter.Configs;
using RateLimiter.Rules;
using RateLimiter.Rules.FixedWindow;
using RateLimiter.Rules.SlidingWindow;
using RateLimiter.Storage;
using RateLimiter.Utilities;
using System.Diagnostics;

/// 100 Calls with in 00:00:20.9636952 Is NOT Allowed
///  50 Calls with in 00:00:10.5412636 Is NOT Allowed
///  20 Calls with in 00:00:04.2662570 Is NOT Allowed
///  15 Calls with in 00:00:03.3027460 Is NOT Allowed
///  10 Calls with in 00:00:02.1793647 Is Allowed

///////log 20 calls with 02.34 seconds.
int calls = 20;
int waitBetweenCallsMS = 100; 

/// user session guid (this can be a token, guid, or a db indx)
Guid SessionID = Guid.NewGuid();

/// configs .json is 10 calls max allowed each 5 seconds
IServiceCollection services = ConfigureServices();
ServiceProvider serviceProvider = services.BuildServiceProvider();

var configs = serviceProvider.GetService<IRateLimiterConfigs>();
var storage = serviceProvider.GetService<IStorage>();

RulesEvaluator rules = new RulesEvaluator();
var limiter = rules.Limiter(new SlidingWindow(configs, storage));


Stopwatch timer = Stopwatch.StartNew();

// log sessions
bool? success = LogSessions(calls, waitBetweenCallsMS, ref limiter, SessionID);

string result = success == true ? "Is Allowed" : "Is NOT Allowed";

Console.WriteLine($"{calls} Calls with in {TimeSpan.FromTicks(timer.ElapsedTicks)} {result}");

timer.Stop();


/// test open for expanstion SOLID
//var fixedWindow = rules.Limiter(new FixedWindow(configs, storage));
//fixedWindow.LogVisitAndEvaluate(Guid.NewGuid());

static bool? LogSessions(int counter, int awaitedMS, ref SlidingWindow limiter, Guid SessionID)
{
    var coll = Enumerable.Range(1, counter).ToList();
    bool? SuccessAccess = false;

    foreach (int i in coll)
    {
        Thread.Sleep(awaitedMS);
        SuccessAccess = limiter?.LogVisitAndEvaluate(SessionID);
    }

    return SuccessAccess;
}


// DI
static IServiceCollection ConfigureServices()
{
    IServiceCollection services = new ServiceCollection();
    _ = services.AddSingleton<IRateLimiterConfigs, RateLimiterConfigs>();
    _ = services.AddSingleton<IDateTimeService, DateTimeService>();
    _ = services.AddSingleton<IStorage, Storage>();
    return services;
}