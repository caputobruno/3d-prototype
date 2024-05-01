using UnityEngine;

public class SearchState : BaseState
{
    float _searchTimer;
    float _searchNewPositionTimer;

    public override void Enter()
    {
        Enemy.Agent.SetDestination(Enemy.PlayerLastKnownPosition);
    }

    public override void Perform()
    {
        if (Enemy.CanSeePlayer())
            StateMachine.ChangeState(new AttackState());

        if (Enemy.Agent.remainingDistance < 0.2f)
            Search();
    }

    void Search()
    {
        _searchTimer += Time.deltaTime;
        _searchNewPositionTimer += Time.deltaTime;

        if (_searchNewPositionTimer > Enemy.SearchNewPositionTime)
        {
            Enemy.Agent.SetDestination(Enemy.PlayerLastKnownPosition + (Random.insideUnitSphere * Enemy.SearchAreaRadius));
            _searchNewPositionTimer = 0;
        }

        if (_searchTimer > Enemy.SearchTime)
            StateMachine.ChangeState(new PatrolState());
    }

    public override void Exit()
    {

    }
}
