using Interfaces;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    ///     Applies damage from an enemy attack to a target object if it supports <see cref="IDamageable" />.
    /// </summary>
    public class EnemyAttack : MonoBehaviour
    {
        public void Attack(GameObject target, int damage)
        {
            var damageable = target.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
            Debug.Log("Damage Dealt: " + damage);
        }
    }
}