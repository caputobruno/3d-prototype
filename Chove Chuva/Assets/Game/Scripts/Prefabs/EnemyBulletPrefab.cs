using UnityEngine;
using UnityEngine.Pool;

public class EnemyBulletPrefab : MonoBehaviour
{
    [SerializeField] float _lifetime = 1f;
    [SerializeField] int _damage = 3;

    float _launchTime;

    Rigidbody _rigBody;
    ObjectPool<EnemyBulletPrefab> _pool;

    void Awake()
    {
        _rigBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Time.time > _launchTime + _lifetime)
            SelfDestruct();
    }

    public void Launch(Vector3 direction, float fireForce)
    {
        //_rigBody.AddForce(Quaternion.AngleAxis(Random.Range(-3, 3), Vector3.up) * direction.normalized * fireForce, ForceMode.Impulse);
        _rigBody.AddForce(direction.normalized * fireForce, ForceMode.Impulse);
        _launchTime = Time.time;
    }

    void SelfDestruct()
    {
        _rigBody.velocity = Vector3.zero;
        _pool.Release(this);
    }

    public void SetPool(ObjectPool<EnemyBulletPrefab> pool) => _pool = pool;

    void OnCollisionEnter(Collision collision)
    {
        var damageable = collision.gameObject.GetComponent<ITakeDamage>();
        damageable?.TakeDamage(_damage);

        SelfDestruct();
    }
}
