using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState _currentState;

    public void Initialize()
    {
        ChangeState(new PatrolState());
    } 

    void Update() => _currentState?.Perform();

    public void ChangeState(BaseState newState)
    {
        if(_currentState == newState) return;

        _currentState?.Exit();

        _currentState = newState;

        if (_currentState != null)
        {
            _currentState.StateMachine = this;
            _currentState.Enemy = GetComponent<Enemy>();
            _currentState.Enter();
        }
    }
}
