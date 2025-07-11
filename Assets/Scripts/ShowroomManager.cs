public class ShowroomManager : AutoSingleton<ShowroomManager>
{
	private const string _showroomKey = "ShowroomSetup";

	public ShowroomSetup CurrentSetup
	{
		get;
		set;
	}

	protected override void OnAwake()
	{
		string @string = Preference.GetString("ShowroomSetup", string.Empty);
		CurrentSetup = new ShowroomSetup(@string);
		base.OnAwake();
	}

	public void SaveLocal()
	{
		string v = CurrentSetup.ToJson();
		Preference.SetString("ShowroomSetup", v);
	}

	public void SaveBackend()
	{
		string json = CurrentSetup.ToJson();
		AutoSingleton<BackendManager>.Instance.SaveShowroom(json);
	}

	public void SetCar(Car car, int slotIndex)
	{
		CurrentSetup.SetCar(car, slotIndex);
		SaveLocal();
	}

	public void SetShowroom(Showroom showroom)
	{
		CurrentSetup.SetShowroom(showroom);
		SaveLocal();
	}
}
