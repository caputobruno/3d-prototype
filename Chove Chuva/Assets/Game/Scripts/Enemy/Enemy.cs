using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateMachine))]
public class Enemy : MonoBehaviour, ITakeDamage
{
    [Header("Enemy Settings")]
    public Path Path;
    [SerializeField] int _health;

    [Header("Sigh Values")]
    [SerializeField] float _sightDistance = 20f;
    [SerializeField] float _fieldOfView = 85f;
    [SerializeField] float _eyeHeight = 1.5f;
    public float WaitBeforeStopAttack;

    [Header("Search Settings")]
    public float SearchTime = 10;
    public float SearchNewPositionTime = 2;
    public float SearchAreaRadius = 5;

    [Header("Weapon")]
    public Transform FirePoint;
    [Range(0.1f, 1f)]
    public float FireRate;
    public float FireForce;

    [System.NonSerialized] public Vector3 PlayerLastKnownPosition;
    public NavMeshAgent Agent { get => _agent; }
    public PlayerController Player {  get; private set; }


    StateMachine _stateMachine;
    NavMeshAgent _agent;

    void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        _agent = GetComponent<NavMeshAgent>();
        _stateMachine.Initialize();
        Player = FindObjectOfType<PlayerController>();
    }

    public bool CanSeePlayer()
    {
        if(Player != null)
        {
            if(Vector3.Distance(transform.position, Player.transform.position) < _sightDistance)
            {
                Vector3 targetDirection = Player.transform.position - transform.position - (Vector3.up * _eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);

                if(angleToPlayer >= -_fieldOfView && angleToPlayer <= _fieldOfView)
                {
                    Ray ray = new(transform.position + (Vector3.up * _eyeHeight), targetDirection);
                    
                    if (Physics.Raycast(ray, out RaycastHit hit, _sightDistance))
                    {
                        if (hit.transform.gameObject == Player.gameObject)
                            return true;
                    }
                }
            }
        }

        return false;
    }

    public void TakeDamage(int damage)
    {
        transform.LookAt(Player.transform.position);

        _health -= damage;
        if(_health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.DeadEnemy();
            UIManager.Instance.UpdateGameplayPanel();
        }
    }
}
