using Systems;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Enemy
{
    /// <summary>
    ///     Respawns an enemy at a configured point whenever its <see cref="Health" /> component signals death.
    ///     Also clears any active <see cref="NavMeshAgent" /> path so AI movement restarts cleanly after respawn.
    /// </summary>
    public class EnemyRespawn : MonoBehaviour
    {
        [FormerlySerializedAs("respawnPoint")] [SerializeField]
        private Transform enemyrespawnPoint;
        
        //TODO:Make enemies have health so they can be killed and actually test respawning lol
        [SerializeField] private Health health;

 
        private void OnEnable()
        {
            health.OnDeath += HandleDeath;
        }
        
        private void OnDisable()
        {
            health.OnDeath -= HandleDeath;
        }
        
        private void HandleDeath()
        {
            transform.position = enemyrespawnPoint.position;
            health.ResetHealth();

            var navAgent = GetComponent<NavMeshAgent>();
            if (navAgent != null) navAgent.ResetPath();
        }
    }
}