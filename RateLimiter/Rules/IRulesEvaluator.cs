namespace RateLimiter.Rules
{
    public interface IRulesEvaluator
    {
        T Limiter<T>(T rule) where T : class;
    }
}