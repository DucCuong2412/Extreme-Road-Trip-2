namespace Roofdog
{
	public class HttpRetryOptions
	{
		public static readonly HttpRetryOptions NO_RETRY = new HttpRetryOptions(12, 16, 0, 0, exponentialBackoff: false);

		public static readonly HttpRetryOptions DEFAULT_RETRY = new HttpRetryOptions(12, 16, 2, 1, exponentialBackoff: true);

		public int RequestTimeoutInSeconds
		{
			get;
			private set;
		}

		public int AbsoluteTimeoutInSeconds
		{
			get;
			private set;
		}

		public int MaxNbRetries
		{
			get;
			private set;
		}

		public int DelayInSeconds
		{
			get;
			private set;
		}

		public bool ExponentialBackoff
		{
			get;
			private set;
		}

		public HttpRetryOptions(int requestTimeoutInSeconds, int absoluteTimeoutInSeconds, int maxNbRetries, int delayInSeconds, bool exponentialBackoff)
		{
			RequestTimeoutInSeconds = requestTimeoutInSeconds;
			AbsoluteTimeoutInSeconds = absoluteTimeoutInSeconds;
			MaxNbRetries = maxNbRetries;
			DelayInSeconds = delayInSeconds;
			ExponentialBackoff = exponentialBackoff;
		}

		public HttpRetryOptions WithRequestTimeoutInSeconds(int seconds)
		{
			return new HttpRetryOptions(seconds, AbsoluteTimeoutInSeconds, MaxNbRetries, DelayInSeconds, ExponentialBackoff);
		}

		public HttpRetryOptions WithAbsoluteTimeoutInSeconds(int seconds)
		{
			return new HttpRetryOptions(RequestTimeoutInSeconds, seconds, MaxNbRetries, DelayInSeconds, ExponentialBackoff);
		}
	}
}
