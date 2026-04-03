using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    /// <summary>
    ///     Respawns an enemy at a configured point whenever its <see cref="Health" /> component signals death.
    ///     Also clears any active <see cref="NavMeshAgent" /> path so AI movement restarts cleanly after respawn.
    /// </summary>
    public class EnemyRespawn : MonoBehaviour
    {
        /// <summary>
        ///     World-space transform used as the enemy's respawn location.
        /// </summary>
        [FormerlySerializedAs("respawnPoint")] [SerializeField]
        private Transform enemyrespawnPoint;

        /// <summary>
        ///     Health component that raises the death event and can be reset after respawn.
        /// </summary>
        //TODO:Make enemies have health so they can be killed and actually test respawning lol
        [SerializeField] private Health health;

        /// <summary>
        ///     Subscribes to the death event when this component is enabled.
        /// </summary>
        private void OnEnable()
        {
            health.OnDeath += HandleDeath;
        }

        /// <summary>
        ///     Unsubscribes from the death event when this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            health.OnDeath -= HandleDeath;
        }

        /// <summary>
        ///     Handles enemy death by moving the enemy to the respawn point,
        ///     restoring health, and resetting navigation if a NavMeshAgent is present.
        /// </summary>
        private void HandleDeath()
        {
            transform.position = enemyrespawnPoint.position;
            health.ResetHealth();

            var navAgent = GetComponent<NavMeshAgent>();
            if (navAgent != null) navAgent.ResetPath();
        }
    }
}