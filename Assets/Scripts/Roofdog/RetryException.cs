using System;

namespace Roofdog
{
	public class RetryException : Exception
	{
		public RetryException(string message)
			: base(message)
		{
		}
	}
}
