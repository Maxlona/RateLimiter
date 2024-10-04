using System;

namespace RateLimiter.Rules.FixedWindow
{
    internal interface IFixedWindow
    {
        bool LogVisitAndEvaluate(Guid SessionID);
    }
}
