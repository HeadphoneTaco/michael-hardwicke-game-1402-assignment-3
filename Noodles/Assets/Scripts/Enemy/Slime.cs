using System;
using System.Collections;
using Enums;
using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Slime : Base
    {
        [SerializeField] private Transform[] patrolPoints;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float chaseDistance;
        [SerializeField] private float chaseCheckAngle;
        [SerializeField] private float giveUpDistance;
        [SerializeField] private Animator enemyAnim;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private float attackCooldown = 5f;

        protected override float ChaseDistance => chaseDistance;
        protected override float AttackRange => attackRange;
        protected override int AttackDamage => attackDamage;

        private float _lastAttackTime;
        private static readonly int AttackHash = Animator.StringToHash("attack");
        private static readonly int DeathHash = Animator.StringToHash("death");
        private static readonly int WalkHash = Animator.StringToHash("walk");
        private static readonly int IdleHash = Animator.StringToHash("idle");

        private Transform _currentTarget;
        private bool _isWaiting;
        private Vector3 _directionToPlayer;

        private void Start()
        {
            Agent = agent;
            EnemyAnim = enemyAnim;
            ChooseARandomPointAndMove();
        }

        private void FixedUpdate()
        {
            if (CurrentState == EnemyState.Idle)
            {
                if (!_isWaiting)
                    StartCoroutine(WaitAndChooseARandomPointAndMove(5));

                if (IsPlayerInRange() && IsInFOV())
                    SetState(EnemyState.Chase);
            }
            else if (CurrentState == EnemyState.Patrol)
            {
                if (agent.remainingDistance <= 0.2f)
                    SetState(EnemyState.Idle);

                if (IsPlayerInRange() && IsInFOV())
                    SetState(EnemyState.Chase);
            }
            else if (CurrentState == EnemyState.Chase)
            {
                agent.SetDestination(playerTransform.position);

                if (IsPlayerInAttackRange())
                    SetState(EnemyState.Attack);

                if (HasPlayerGoneAwayFromMeTooSad())
                    SetState(EnemyState.Idle);
            }
            else if (CurrentState == EnemyState.Attack)
            {
                agent.SetDestination(transform.position);

                if (!IsPlayerInAttackRange())
                    SetState(EnemyState.Chase);

                if (Time.time >= _lastAttackTime + attackCooldown)
                {
                    _lastAttackTime = Time.time;
                    HandleAttack();
                }
            }
        }

        public void ResetEnemy()
        {
            _isWaiting = false;
            SetState(EnemyState.Idle);
        }

        protected override void SetState(EnemyState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;

            switch (CurrentState)
            {
                case EnemyState.Idle:
                    EnemyAnim.SetBool(WalkHash, false);
                    EnemyAnim.SetBool(IdleHash, true);
                    EnemyAnim.SetBool(AttackHash, false);
                    break;

                case EnemyState.Patrol:
                case EnemyState.Chase:
                    EnemyAnim.SetBool(IdleHash, false);
                    EnemyAnim.SetBool(WalkHash, true);
                    EnemyAnim.SetBool(AttackHash, false);
                    break;

                case EnemyState.Attack:
                    EnemyAnim.SetBool(WalkHash, false);
                    EnemyAnim.SetBool(IdleHash, false);
                    EnemyAnim.SetBool(AttackHash, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void HandleAttack()
        {
            var damageable = playerTransform.GetComponentInParent<IDamageable>();
            damageable?.TakeDamage(AttackDamage);
        }

        private bool IsPlayerInAttackRange()
        {
            return Vector3.Distance(transform.position, playerTransform.position) <= attackRange;
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
            if (!agent.isOnNavMesh) return;
            _currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
            agent.SetDestination(_currentTarget.position);
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