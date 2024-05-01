using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    public int WaypointIndex;
    [SerializeField] float _waitTime = 3;

    float _waitTimer;

    public override void Enter()
    {

    }
    public override void Perform()
    {
        PatrolCycle();
        if (Enemy.CanSeePlayer())
            StateMachine.ChangeState(new AttackState());
    }
    public override void Exit()
    {

    }

    public void PatrolCycle()
    {
        if (Enemy.Agent.remainingDistance < 0.2f)
        {
            _waitTimer += Time.deltaTime;
            if(_waitTimer > _waitTime)
            {
                _waitTimer = 0;

                WaypointIndex = WaypointIndex < Enemy.Path.Waypoints.Count - 1 ? WaypointIndex + 1 : 0;

                Enemy.Agent.SetDestination(Enemy.Path.Waypoints[WaypointIndex].position);

            }
        }
    }
}
