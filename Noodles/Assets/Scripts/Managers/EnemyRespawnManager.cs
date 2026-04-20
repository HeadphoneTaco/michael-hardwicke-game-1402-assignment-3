using System.Collections;
using Systems;
using UnityEngine;
using UnityEngine.AI;

namespace Managers
{
    /// <summary>
    ///     Respawns an enemy at a configured point whenever its <see cref="Health" /> component signals death.
    ///     Also clears any active <see cref="NavMeshAgent" /> path so AI movement restarts cleanly after respawn.
    /// </summary>
    public class EnemyRespawnManager : MonoBehaviour
    {
        [SerializeField] private Transform enemyRespawnPoint;
        [SerializeField] private Health health;
        [SerializeField] private float respawnDelay = 3f;
        
        private NavMeshAgent _navAgent;
        private Enemy.Slime _slime;
        private Animator _animator;
        private static readonly int Death = Animator.StringToHash("death");
        private bool _isDead;


        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _slime = GetComponent<Enemy.Slime>();
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
            if (_isDead) return;       // ignore repeat calls
            _isDead = true;
            StartCoroutine(DeathSequence());
        }
        
        private IEnumerator DeathSequence()
        {
            // Disable AI so enemy stops moving
            _navAgent.enabled = false;
            _slime.enabled = false;
            _animator?.SetTrigger(Death);
            //_animator?.ResetTrigger(Death);
            yield return new WaitForSeconds(respawnDelay);
            health.ResetHealth();
            _navAgent.enabled = true;
            _navAgent.Warp(enemyRespawnPoint.position); 
            _navAgent.ResetPath();
            _isDead = false;
            _slime.enabled = true;
            _slime.ResetEnemy();
            _animator?.ResetTrigger(Death);
            yield return null;   
            _animator?.Play("idle", 0, 0f);
        }
    }
}