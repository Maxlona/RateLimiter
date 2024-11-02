**Rate-limiter Solution Overview...**

- Rate limiting controls the number of requests a client can make within a set period. Each client is identified by an access token (GUID) used for every request to the resource.
- To prevent server overload and abuse, APIs implement rate-limiting strategies. These strategies help determine whether to allow or block incoming requests based on the client’s activity.
- When a client makes an API request, the server checks if the request falls within the allowed rate limit. If it does, the request is processed; otherwise, it is blocked to prevent further requests.
- The RateLimiter follows the SOLID "Open for Extension" principle, allowing the configuration of various limiting algorithms, such as the Leaky Bucket or Fixed Window methods.
- NSubstitute is used to mock and test dependencies within the RateLimiter, ensuring the functionality can be effectively verified.

**More Info!**

**What is a Rate Limiter?**
A rate limiter is a mechanism that restricts the number of requests a user or system can make to a service within a specified time frame. This helps protect applications from being overwhelmed by too many requests, prevents abuse, and ensures fair access for all users. Rate limiting is often used in APIs, web services, and distributed systems to maintain performance and avoid bottlenecks.

**Why Use a Rate Limiter?**
Prevents Overloading: By controlling the frequency of requests, rate limiting prevents overloading your application or API.
Improves Performance and Reliability: Ensures the application remains responsive by controlling the traffic it receives.
Protects Against Abuse: Prevents misuse or abuse of APIs by limiting the rate at which requests can be made by any single user or IP address.
Ensures Fair Access: Provides all users with fair access to the service by distributing resources equitably.
How Does Rate Limiting Work?
There are several algorithms used for implementing rate limiting. Each has its own strengths and ideal use cases. Popular algorithms include:

**Fixed Window:** Counts requests within fixed intervals (e.g., every minute).
**Leaky Bucket:** Queues requests and processes them at a fixed rate.
**Token Bucket:** Limits based on available tokens, allowing bursts of requests.
**Sliding Window:** Offers a more dynamic way of measuring requests over time by using a rolling time window.
This README focuses on Sliding Window Rate Limiting.

**How Sliding Window Works**
Imagine we want to allow 100 requests per minute for each user. Instead of counting all requests within a single minute block, the sliding window splits this minute into smaller intervals (e.g., seconds) and "slides" to accommodate requests made in real time. This reduces the chance of a burst of requests just before the minute ends, which could cause rate-limiting issues.

**The Sliding Window algorithm works as follows:**
Divide Time into Smaller Intervals: Split the time window (e.g., 1 minute) into smaller segments (e.g., seconds).
Track Requests per Interval: Track requests for each small interval.
Slide the Window: As each new request comes in, adjust the window to consider only requests within the last full window (e.g., last 60 seconds).
Sum the Requests: Sum the requests in the current window. If the total requests exceed the limit (e.g., 100 requests), the rate limit is exceeded, and the request is denied.
Update as Time Progresses: As time progresses, drop intervals that fall outside the current window.
Example Scenario
Suppose we have a 60-second window with a limit of 100 requests per minute. Each time a request arrives, the algorithm checks all requests made in the past 60 seconds. If fewer than 100 requests have been made, the new request is accepted. Otherwise, it is denied.
