using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateLimiter.Rules.FixedWindow
{
    internal interface ISlidingWindow
    {
        bool LogVisitAndEvaluate(Guid SessionID);
    }
}
