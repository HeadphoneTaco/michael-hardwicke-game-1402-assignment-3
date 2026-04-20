using System.Collections;
using Enemy;
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
        private static readonly int Death = Animator.StringToHash("death");
        [SerializeField] private Transform enemyRespawnPoint;
        [SerializeField] private Health health;
        [SerializeField] private float respawnDelay = 3f;
        private Animator _animator;
        private bool _isDead;

        private NavMeshAgent _navAgent;
        private Slime _slime;


        private void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _slime = GetComponent<Slime>();
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
            if (_isDead) return; // ignore repeat calls
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