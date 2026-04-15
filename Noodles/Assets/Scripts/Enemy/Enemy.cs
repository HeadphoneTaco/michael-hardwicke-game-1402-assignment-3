using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;

namespace Enemy
{
    /// <summary>
    ///     Controls enemy patrol behavior using a simple state machine.
    ///     The enemy moves between random patrol points, pauses when it arrives,
    ///     then selects a new destination.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float chaseDistance;
        [SerializeField] private float giveUpDistance;
        [SerializeField] private float chaseCheckAngle;
        [SerializeField] private Animator enemyAnim;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackCooldown = 1.5f;
        private float _lastAttackTime;
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int Death = Animator.StringToHash("death");
        private EnemyState _currentState;
        private Transform _currentTarget;
        private bool _isWaiting;
        private static readonly int Walk = Animator.StringToHash("walk");
        private static readonly int Idle = Animator.StringToHash("idle");
        private Vector3 _directionToPlayer;
        private void Start()
        {
            ChooseARandomPointAndMove();
        }
        private void FixedUpdate()
        {
            if (_currentState == EnemyState.Idle)
            {
                if (!_isWaiting)
                    StartCoroutine(WaitAndChooseARandomPointAndMove(5));
        
                if (IsPlayerInRange() && IsInFOV())
                    SetState(EnemyState.Chase);
            }
            else if (_currentState == EnemyState.Patrol)
            {
                if (agent.remainingDistance <= 0.2f)
                    SetState(EnemyState.Idle);
        
                if (IsPlayerInRange() && IsInFOV())
                    SetState(EnemyState.Chase);
            }
            else if (_currentState == EnemyState.Chase)
            {
                agent.SetDestination(playerTransform.position);
    
                if (IsPlayerInAttackRange())
                    SetState(EnemyState.Attack);
    
                if (HasPlayerGoneAwayFromMeTooSad())
                    SetState(EnemyState.Idle);
            }
            else if (_currentState == EnemyState.Attack)
            {
                agent.SetDestination(transform.position); // stop moving
    
                if (!IsPlayerInAttackRange())
                    SetState(EnemyState.Chase);
    
                if (Time.time >= _lastAttackTime + attackCooldown)
                {
                    _lastAttackTime = Time.time;
                    TryDamagePlayer();
                }
            }
        }
        
        public void ResetEnemy()
        {
            SetState(EnemyState.Idle);
        }
        
        private void SetState(EnemyState newState)
        {
            // Don't do anything if we're already in this state
            if (_currentState == newState) return;
    
            _currentState = newState;
            Debug.Log("Current State: " + _currentState);
    
            switch (_currentState)
            {
                case EnemyState.Idle:
                    enemyAnim.SetBool(Walk, false);
                    enemyAnim.SetBool(Idle, true);
                    enemyAnim.SetBool(Attack, false);
                    break;
            
                case EnemyState.Patrol:
                    enemyAnim.SetBool(Idle, false);
                    enemyAnim.SetBool(Walk, true);
                    enemyAnim.SetBool(Attack, false);
                    break;
            
                case EnemyState.Chase:
                    enemyAnim.SetBool(Idle, false);
                    enemyAnim.SetBool(Walk, true);
                    enemyAnim.SetBool(Attack, false);
                    break;
                
                case EnemyState.Attack:
                    enemyAnim.SetBool(Walk, false);
                    enemyAnim.SetBool(Idle, false);
                    enemyAnim.SetBool(Attack, true);
                    break;
            }
        }
        
        private bool IsPlayerInAttackRange()
        {
            return Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
        }

        private void TryDamagePlayer()
        {
            if (playerTransform.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(attackDamage);
        }
        
        private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
        {
            _isWaiting = true;
            yield return new WaitForSeconds(timeToWait);
            SetState(EnemyState.Patrol);
            ChooseARandomPointAndMove();
            _isWaiting = false;
        }
        private void ChooseARandomPointAndMove()
        {
            if (patrolPoints.Length <= 0) return;

            _currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
            agent.SetDestination(_currentTarget.position);
            Debug.Log("Random point chosen");
        }
        private bool IsPlayerInRange()
        {
            return Vector3.Distance(transform.position, playerTransform.position) <= chaseDistance;
        }
        private bool HasPlayerGoneAwayFromMeTooSad()
        {
            return Vector3.Distance(transform.position, playerTransform.position) >= giveUpDistance;
        }
        private bool IsInFOV()
        {
            _directionToPlayer = (playerTransform.position - transform.position).normalized;
            return Vector3.Angle(transform.forward, _directionToPlayer) <= chaseCheckAngle;
        }
    }
}