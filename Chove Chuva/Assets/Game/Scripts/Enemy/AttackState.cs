using UnityEngine;

public class AttackState : BaseState
{
    float _moveTimer;
    float _losePlayerTimer;
    float _shotTimer;

    public override void Enter()
    {

    }

    public override void Perform()
    {
        if(Enemy.CanSeePlayer())
        {
            Enemy.transform.LookAt(Enemy.Player.transform.position);

            _losePlayerTimer = 0;
            _moveTimer += Time.deltaTime;
            _shotTimer += Time.deltaTime;

            if (_moveTimer > Random.Range(3, 7))
            {
                Enemy.Agent.SetDestination(Enemy.transform.position + (Random.insideUnitSphere * 5));
                _moveTimer = 0;
            }

            if (_shotTimer > Enemy.FireRate)
                Shoot();

            Enemy.PlayerLastKnownPosition = Enemy.Player.transform.position;
        }
        else
        {
            _losePlayerTimer += Time.deltaTime;

            if(_losePlayerTimer > Enemy.WaitBeforeStopAttack)
                StateMachine.ChangeState(new SearchState());
        }
    }

    public void Shoot()
    {
        Vector3 shotDirection = (Enemy.Player.transform.position - Enemy.FirePoint.transform.position);

        EnemyBulletPrefab bullet = PoolManager.Instance.GetEnemyBullet();
        bullet.transform.position = Enemy.FirePoint.position;
        bullet.Launch(shotDirection, Enemy.FireForce);

        _shotTimer = 0;
    }

    public override void Exit()
    {

    }
}