using RateLimiter.Configs;
using RateLimiter.Models;
using RateLimiter.Rules.FixedWindow;
using RateLimiter.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RateLimiter.Rules.FixedWindow
{
    public class FixedWindow : IFixedWindow
    {
        IRateLimiterConfigs config;
        IStorage store;
        public FixedWindow(IRateLimiterConfigs _config, IStorage _store)
        {
            store = _store;
            config = _config;
        }

        /// <summary>
        /// if the user send more calls than max allowed PER timeframe window 
        /// ex: 11 calls per 3 seconds,where the rules are 10 max calls per 5 seconds
        /// CanAccess should return false
        /// </summary>
        /// <param name="start">first calls in a timeframe</param>
        /// <param name="last">last call recorded in a timeframe</param>
        /// <param name="totalCalls">number of calls per timeframe</param>
        /// <returns></returns>
        private bool CanAccess(List<DateTime>? Visits)
        {
            throw new NotImplementedException();
        }

        public bool LogVisitAndEvaluate(Guid SessionID)
        {
            throw new NotImplementedException();
        }
    }
}
