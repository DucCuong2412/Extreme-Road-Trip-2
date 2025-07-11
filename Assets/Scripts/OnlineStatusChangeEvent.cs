using System;

public struct OnlineStatusChangeEvent
{
	public bool IsOnline;

	public bool WasOffline;

	public TimeSpan BackendTimeDelta;
}
