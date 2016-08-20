using System;
using System.Collections.Generic;


public interface FSMState<T>
{
	Int32 State { get; }
	void Start(T o);
	void End(T o);
	bool CanTransition(T o, FSMState<T> state);
	void TransitionCompleted(T o, FSMState<T> state);
}

public class FSMMachine<T> : IDestroyable
{
	private T _fsmObject;
	private Int32 _currentState = 0;
	private FSMState<T>[] _fsmStates = null;
	//private ViFramEndCallback3<FSMState<T>, FSMState<T>, Int32> _asyncCallback = new ViFramEndCallback3<FSMState<T>, FSMState<T>, int>();

	public T FsmObject { get { return _fsmObject; } }
	public Int32 CurrentState { get { return _currentState; } set { _currentState = value; } }

	public void Start(T fsmObject, int size)
	{
		_fsmObject = fsmObject;
		_fsmStates = new FSMState<T>[size];
	}

	public void Destroy()
	{
		_fsmObject = default(T);
		for (Int32 i = 0; i < _fsmStates.Length; ++i)
		{
			_fsmStates[i] = null;
		}
		_fsmStates = null;
	}

	public void AddState(int idx, FSMState<T> state)
	{
		if (idx < _fsmStates.Length)
		{
			_fsmStates[idx] = state;
		}
	}

	public FSMState<T> GetState(int idx)
	{
		if (idx < _fsmStates.Length)
		{
			return _fsmStates[idx];
		}
		else
		{
			return null;
		}
	}

	public void ForceChangeToState(Int32 newState)
	{
		if (newState >= _fsmStates.Length)
		{
			return;
		}
		Int32 oldState = CurrentState;
		FSMState<T> oldFsmState = _fsmStates[oldState];
		FSMState<T> newFsmState = _fsmStates[newState];
		if (oldFsmState.CanTransition(_fsmObject, newFsmState))
		{
			OnAsyncCallback(oldFsmState, newFsmState, newState);
		}
	}

	public void ChangeToState(Int32 newState)
	{
		if (newState >= _fsmStates.Length)
		{
			return;
		}
		Int32 oldState = CurrentState;
		FSMState<T> oldFsmState = _fsmStates[oldState];
		FSMState<T> newFsmState = _fsmStates[newState];
		if (oldFsmState.CanTransition(_fsmObject, newFsmState))
		{
			OnAsyncCallback(oldFsmState,newFsmState,newState);
		}
	}

	void OnAsyncCallback(FSMState<T> oldFsmState, FSMState<T> newFsmState, Int32 newState)
	{
		oldFsmState.End(_fsmObject);
		CurrentState = newState;
		newFsmState.Start(_fsmObject);

		newFsmState.TransitionCompleted(_fsmObject, oldFsmState);
	}
}
