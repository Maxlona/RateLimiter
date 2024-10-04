using RateLimiter.Configs;
using RateLimiter.Models;
using RateLimiter.Rules.FixedWindow;
using RateLimiter.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace RateLimiter.Rules.SlidingWindow
{
    public class SlidingWindow : ISlidingWindow
    {
        IRateLimiterConfigs config;
        IStorage store;
        public SlidingWindow(IRateLimiterConfigs _config, IStorage _store)
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
        public bool CanAccess(List<DateTime>? Visits)
        {
            var configs = config.BindConfig();

            /// isEnabled
            if (configs?.Enabled == true)
            {
                if (configs.MaxAllowed.HasValue && configs.PerSecondsTimeFrame.HasValue)
                {
                    int MaxAllowedCalls = configs.MaxAllowed.Value;
                    int PerSecondsTimeFrame = configs.PerSecondsTimeFrame.Value;

                    // get last x seconds total calls
                    PerSecondsTimeFrame *= -1; // get negative of that value
                    var total = Visits?.Where(e => e > DateTime.Now.AddSeconds(PerSecondsTimeFrame)).ToList();
                    
                    // evaluate
       
                    /// the user timespan was within limit to evaluate
                    if (total.Count > MaxAllowedCalls)
                    {
                        // user called the APIs > listed in config 
                        return false;
                    }
                    return true;
                }
            }

            return true;
        }

        public bool LogVisitAndEvaluate(Guid SessionID)
        {
            store.AddOrAppend(SessionID);
            var visits = store.Get(SessionID);
            
            // if has data, evaluate
            if (visits != null && visits.Count > 1)
            {
                visits?.Sort();

                bool? canAccess = CanAccess(visits);

                if(canAccess == true)
                {
                }

                return canAccess.Value;
            }
            else
            {
                // too little to compare
                return true;
            }
        }
    }
}
