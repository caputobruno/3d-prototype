using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] BulletPrefab _playerBulletPrefab;
    [SerializeField] EnemyBulletPrefab _enemyBulletPrefab;
    
    ObjectPool<BulletPrefab> _playerBulletPool;
    ObjectPool<EnemyBulletPrefab> _enemyBulletPool;

    public static PoolManager Instance;

    void Awake()
    {
        Instance = this;

        _playerBulletPool = new ObjectPool<BulletPrefab>(
            () =>
            {
                var shot = Instantiate(_playerBulletPrefab);
                shot.SetPool(_playerBulletPool);
                return shot;
            },
            t => t.gameObject.SetActive(true),
            t => t.gameObject.SetActive(false));

        _enemyBulletPool = new ObjectPool<EnemyBulletPrefab>(
            () =>
            {
                var shot = Instantiate(_enemyBulletPrefab);
                shot.SetPool(_enemyBulletPool);
                return shot;
            },
            t => t.gameObject.SetActive(true),
            t => t.gameObject.SetActive(false));
    }

    public BulletPrefab GetPlayerBullet() => _playerBulletPool.Get();
    public EnemyBulletPrefab GetEnemyBullet() => _enemyBulletPool.Get();
}
