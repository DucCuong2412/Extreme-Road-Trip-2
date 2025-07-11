using UnityEngine;

public class StateMachine : MonoBehaviour
{
	private State _previous;

	private State _current;

	private State _next;

	public State GetPrevious()
	{
		return _previous;
	}

	public State GetCurrent()
	{
		return _current;
	}

	public State GetNext()
	{
		return _next;
	}

	public void Change(State state)
	{
		_next = state;
	}

	public void Execute(float dt)
	{
		MaybeChangeState();
		if (_current != null)
		{
			_current.OnExecute(dt);
		}
		MaybeChangeState();
	}

	private void MaybeChangeState()
	{
		if (_next != null)
		{
			if (_current != null)
			{
				_current.OnExit();
			}
			_previous = _current;
			_current = _next;
			_next = null;
			_current.OnEnter();
		}
	}
}
