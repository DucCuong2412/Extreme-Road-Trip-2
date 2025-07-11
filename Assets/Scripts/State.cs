public class State
{
	protected StateMachine SM;

	public State(StateMachine sm)
	{
		SM = sm;
	}

	public virtual void OnEnter()
	{
	}

	public virtual void OnExit()
	{
	}

	public virtual void OnExecute(float dt)
	{
	}
}
