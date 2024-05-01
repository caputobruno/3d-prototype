using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] BulletPrefab _bulletPrefab;
    public Transform FirePoint;
    [SerializeField] float _bulletForce;
    [SerializeField] float _fireRate;
    float _lastFireTime;

    bool _canShoot;

    void Awake()
    {
        _canShoot = true;
    }

    public void Fire()
    {
        if (Time.time > _lastFireTime + _fireRate)
            _canShoot = true;

        if (!_canShoot) return;

        _lastFireTime = Time.time;
        _canShoot = false;

        //var bullet = Instantiate(_bulletPrefab, FirePoint.position, Quaternion.identity);
        BulletPrefab bullet = PoolManager.Instance.GetPlayerBullet();
        bullet.transform.position = FirePoint.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.Launch(FirePoint.forward, _bulletForce);

    }
}
