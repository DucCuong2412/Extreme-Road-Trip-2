using System.Collections;
using System.Collections.Generic;

public struct XPLevel
{
	public float _xpRequired;

	public List<Reward> _rewards;

	public XPLevel(float xpRequired, ArrayList jsonArray)
	{
		_xpRequired = xpRequired;
		_rewards = Reward.FromJsonData(jsonArray);
	}
}
