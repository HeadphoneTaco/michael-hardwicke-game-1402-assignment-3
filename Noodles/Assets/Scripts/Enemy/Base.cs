using Enums;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public abstract class Base : MonoBehaviour
    {
        protected NavMeshAgent Agent;
        protected EnemyState CurrentState;
        protected Animator EnemyAnim;

        protected abstract float ChaseDistance { get; }
        protected abstract float AttackRange { get; }
        protected abstract int AttackDamage { get; }

        protected abstract void HandleAttack();
        protected abstract void SetState(EnemyState newState);
    }
}