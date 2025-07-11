public class VisualXPProfile : XPProfile
{
	public VisualXPProfile(float xp)
		: base(xp)
	{
	}

	public override void RegisterXP(float xp)
	{
		base.XP += xp;
	}
}
