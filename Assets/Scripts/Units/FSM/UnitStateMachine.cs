using System.Collections.Generic;
using UnityEngine;

public class UnitStateMachine<T, U>
{
    private readonly Dictionary<U, State<T, U>> _stateDictionary = new();
    private readonly T _owner;
    
    private State<T, U> _currentState;
    private State<T, U> _previousState;

    public UnitStateMachine(T owner)
    {
        _owner = owner;
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Execute(_owner);
        }
    }

    public void ChangeState(U newState)
    {
        _previousState = _currentState;

        if (_currentState != null)
        {
            _currentState.Exit(_owner);
        }

        if (_stateDictionary.ContainsKey(newState)) 
        {
            _currentState = _stateDictionary[newState];

            if (_currentState != null)
            {
                _currentState.Enter(_owner);
            }
        } 
        else 
        {
            Debug.LogError(newState + " is not Registered.");
        }
    }

    public State<T, U> TryGetState(U state)
    { 
        if (!_stateDictionary.ContainsKey(state)) 
        {
            Debug.LogError(state + " is not Registered.");
        } 
        
        return _stateDictionary[state];
    }
    
    public void RevertToPreviousState()
    {
        if (_previousState != null)
        {
            ChangeState(_previousState.Id);
        }
    }

    public void Subscribe(State<T, U> state)
    {
        _stateDictionary.Add(state.Id, state);
    }

    public void Subscribe(List<State<T, U>> stateList)
    {
        foreach (var state in stateList) 
        { 
            Subscribe(state);
        }
    }

    public void Unsubscribe(State<T, U> state)
    {
        _stateDictionary.Remove(state.Id);
    }
}