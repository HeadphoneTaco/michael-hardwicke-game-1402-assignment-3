using System.Collections;
using Systems;
using UnityEngine;
using UnityEngine.AI;


namespace Enemy
{
    /// <summary>
    ///     Respawns an enemy at a configured point whenever its <see cref="Health" /> component signals death.
    ///     Also clears any active <see cref="NavMeshAgent" /> path so AI movement restarts cleanly after respawn.
    /// </summary>
    public class EnemyRespawn : MonoBehaviour
    {
        [SerializeField] private Transform enemyRespawnPoint;
        [SerializeField] private Health health;
        [SerializeField] private float respawnDelay = 3f;
        
        private NavMeshAgent _navAgent;
        private Enemy _enemy;
        private Animator _animator;
        private static readonly int Death = Animator.StringToHash("death");


        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _enemy = GetComponent<Enemy>();
            _animator = GetComponent<Animator>();
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
            StartCoroutine(DeathSequence());
        }
        
        private IEnumerator DeathSequence()
        {
            // Disable AI so enemy stops moving
            _navAgent.enabled = false;
            _enemy.enabled = false;
            
            
            _animator?.SetTrigger(Death);
            
            yield return new WaitForSeconds(respawnDelay);
            
            // Reset position
            transform.position = enemyRespawnPoint.position;

            // Reset health and re-enable AI
            health.ResetHealth();
            _navAgent.enabled = true;
            _enemy.enabled = true;
            _navAgent.ResetPath();

            // Reset enemy back to idle
            _enemy.ResetEnemy();

            // Reset animator
            _animator?.ResetTrigger(Death);
        }
    }
}