using _Project.Scripts.Interfaces;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Applies damage from an enemy attack to a target object if it supports <see cref="IDamageable" />.
    ///     Intended to be invoked from a Visual Scripting graph.
    /// </summary>
    public class EnemyAttack : MonoBehaviour
    {
        /// <summary>
        ///     Attempts to damage the target by retrieving its <see cref="IDamageable" /> component
        ///     and invoking <see cref="IDamageable.TakeDamage(int)" /> with the provided amount.
        /// </summary>
        /// <param name="target">The GameObject being attacked.</param>
        /// <param name="damage">The amount of damage to apply.</param>
        // Called directly from the Visual Scripting graph
        // Damage is passed in from the graph, keeping this script generic
        public void Attack(GameObject target, int damage)
        {
            var damageable = target.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
            Debug.Log("Damage Dealt: " + damage);
        }
    }
}