using System;
using System.Collections;
using System.Runtime.CompilerServices;

public abstract class Mission
{
	private string _description;

	public bool Completed
	{
		get;
		set;
	}

	public string Id
	{
		get;
		set;
	}

	public float Objective
	{
		get;
		set;
	}

	public int Priority
	{
		get;
		private set;
	}

	public string Description
	{
		get
		{
			return _description.Localize();
		}
		set
		{
			_description = value;
		}
	}

	[method: MethodImpl(32)]
	public event Action<Mission> OnMissionCompletedEvent;

	public abstract void RegisterEvents(Car car);

	public abstract void UnRegisterEvents();

	public virtual void Load(Hashtable jsonTable)
	{
		Id = JsonUtil.ExtractString(jsonTable, "id", string.Empty);
		Description = JsonUtil.ExtractString(jsonTable, "description", string.Empty);
		Objective = JsonUtil.ExtractInt(jsonTable, "objective", 0);
		Priority = JsonUtil.ExtractInt(jsonTable, "priority", 0);
	}

	public virtual Hashtable GetCustomData()
	{
		return null;
	}

	public virtual void SetCustomData(Hashtable customData)
	{
	}

	public virtual void OnSetCurrent(Car currentCar)
	{
	}

	public virtual string GetRemainingValueBeforeCompletion(Car currentCar)
	{
		return string.Empty;
	}

	public void Reset()
	{
		Completed = false;
	}

	protected void OnMissionCompleted()
	{
		UnRegisterEvents();
		Completed = true;
		if (this.OnMissionCompletedEvent != null)
		{
			this.OnMissionCompletedEvent(this);
		}
	}

	public override string ToString()
	{
		return Description;
	}
}
