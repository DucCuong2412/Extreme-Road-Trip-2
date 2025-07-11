using System.Collections;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
	public Transform _frame;

	protected bool _isAnimating;

	protected bool _finished;

	public void Start()
	{
		_isAnimating = true;
	}

	public void Stop()
	{
		_isAnimating = false;
	}

	protected abstract void Reset();

	protected abstract IEnumerator AnimCR();

	protected virtual void Awake()
	{
		_finished = true;
	}

	protected virtual void FixedUpdate()
	{
		if (_finished)
		{
			_finished = false;
			Reset();
			StartCoroutine(AnimCR());
		}
	}
}
