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
        private Transform _enemyRespawnPoint;
        
        //TODO:Make enemies have health so they can be killed and actually test respawning lol
        [SerializeField] private Health health;
        private NavMeshAgent _navAgent;


        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
        }

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
            transform.position = _enemyRespawnPoint.position;
            health.ResetHealth();

            _navAgent?.ResetPath();
        }
    }
}