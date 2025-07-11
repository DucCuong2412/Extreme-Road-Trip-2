public static class ProcessMessageGate
{
	public static bool DisplayMessage(MetroMenu menu)
	{
		if (AutoSingleton<MetroMenuStack>.IsCreated() && AutoSingleton<MetroMenuStack>.Instance.Peek() == menu)
		{
			if (ServerRewardUIHandler.ProcessServerReward())
			{
				return true;
			}
			return AutoSingleton<MessageHandler>.Instance.ProcessServerMessage();
		}
		return false;
	}
}
