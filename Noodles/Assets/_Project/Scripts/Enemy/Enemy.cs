using System.Collections;
using _Project.Scripts.Enums;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts
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
        private EnemyState currentState;
        private Transform currentTarget;
        private bool isWaiting;
        private static readonly int Walk = Animator.StringToHash("walk");
        private static readonly int Idle = Animator.StringToHash("idle");
        private Vector3 directionToPlayer;
        private void Start()
        {
            ChooseARandomPointAndMove();
        }
        private void FixedUpdate()
        {
            if (currentState == EnemyState.Idle)
            {
                enemyAnim.SetBool(Idle, true);
                Debug.Log("Current State: " + currentState);
                if (!isWaiting)
                {
                    StartCoroutine(WaitAndChooseARandomPointAndMove(5));

                    //check for the player to chase
                    if (IsPlayerInRange() && IsInFOV())
                    {
                        currentState = EnemyState.Chase;
                        enemyAnim.SetBool(Idle, false);
                        Debug.Log("Current State: " + currentState);
                    }
                }
            }
            else if (currentState == EnemyState.Patrol)
            {
                enemyAnim.SetBool(Walk, true);
                Debug.Log("Current State: " + currentState);

                if (agent.remainingDistance <= 0.2f)
                {
                    currentState = EnemyState.Idle;
                    enemyAnim.SetBool(Walk, false);
                    Debug.Log("Current State: " + currentState);
                }

                //check for the player to chase
                if (IsPlayerInRange() && IsInFOV())
                {
                    currentState = EnemyState.Chase;
                    enemyAnim.SetBool(Walk, true);
                    Debug.Log("Current State: " + currentState);
                }


                else if (currentState == EnemyState.Chase)
                {
                    agent.SetDestination(playerTransform.position);
                    enemyAnim.SetBool(Walk, false);

                    //give up
                    if (HasPlayerGoneAwayFromMeTooSad())
                    {
                        currentState = EnemyState.Idle;
                        enemyAnim.SetBool(Walk, false);
                        Debug.Log("Current State: " + currentState);
                    }
                }
            }
        }
        private IEnumerator WaitAndChooseARandomPointAndMove(float timeToWait)
        {
            isWaiting = true;
            yield return new WaitForSeconds(timeToWait);
            currentState = EnemyState.Patrol;
            enemyAnim.SetBool(Idle, false);
            Debug.Log("Current State: " + currentState);
            ChooseARandomPointAndMove();
            isWaiting = false;
        }
        private void ChooseARandomPointAndMove()
        {
            if (patrolPoints.Length <= 0) return;

            currentTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
            agent.SetDestination(currentTarget.position);
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
            directionToPlayer = (playerTransform.position - transform.position).normalized;
            return Vector3.Angle(transform.forward, directionToPlayer) <= chaseCheckAngle;
        }

    }
}