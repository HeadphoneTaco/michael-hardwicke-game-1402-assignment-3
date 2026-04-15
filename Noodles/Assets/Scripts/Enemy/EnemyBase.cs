using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected NavMeshAgent Agent;
        protected Animator EnemyAnim;
        protected EnemyState CurrentState;

        protected abstract float ChaseDistance { get; }
        protected abstract float AttackRange { get; }
        protected abstract int AttackDamage { get; }

        protected abstract void HandleAttack();
        protected abstract void SetState(EnemyState newState);
    }
}