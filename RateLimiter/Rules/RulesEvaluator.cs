namespace RateLimiter.Rules
{
    public class RulesEvaluator : IRulesEvaluator
    {
        public T Limiter<T>(T rule) where T : class
        {
            switch (typeof(T))
            {
                case
                    var cls when cls == typeof(SlidingWindow.SlidingWindow):
                    {
                        var temp = (SlidingWindow.SlidingWindow)((object)rule);
                        return rule;
                    }
                case
                    var cls when cls == typeof(FixedWindow.FixedWindow):
                    {
                        var temp = (FixedWindow.FixedWindow)((object)rule);
                        return rule;
                    }
            }
            return rule;
        }
    }
}
