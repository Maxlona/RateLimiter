**Rate-limiting pattern**

Rate limiting controls the number of requests a client can make within a set period. Each client is identified by an access token (GUID) used for every request to the resource.

To prevent server overload and abuse, APIs implement rate-limiting strategies. These strategies help determine whether to allow or block incoming requests based on the client’s activity.

When a client makes an API request, the server checks if the request falls within the allowed rate limit. If it does, the request is processed; otherwise, it is blocked to prevent further requests.

The RateLimiter follows the SOLID "Open for Extension" principle, allowing the configuration of various limiting algorithms, such as the Leaky Bucket or Fixed Window methods.

NSubstitute is used to mock and test dependencies within the RateLimiter, ensuring the functionality can be effectively verified.