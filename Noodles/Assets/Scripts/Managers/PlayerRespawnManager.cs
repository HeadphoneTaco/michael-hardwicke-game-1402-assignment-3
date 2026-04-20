using System.Collections;
using Player;
using Systems;
using UnityEngine;

namespace Managers
{
    /// <summary>
    ///     Death and respawn flow, including death effects, delay, and repositioning
    ///     to spawn point.
    /// </summary>
    public class PlayerRespawnManager : MonoBehaviour
    {
        [SerializeField] private PlayerController controller;
        [SerializeField] private Animator animator;
        [SerializeField] private SpawnPoint activeSpawnPoint;
        [SerializeField] private GameObject deathSpritePrefab;
        [SerializeField] private Health health;
        [SerializeField] private float deathDelay = 2f;
        private GameObject _activeGhost;
        private static PlayerRespawnManager _instance;
        private static readonly int Respawn = Animator.StringToHash("Respawn");
        private static readonly int Death = Animator.StringToHash("Death");

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }
        
        private void OnDisable()
        {
            health.OnDeath -= HandleDeath;
        }

        private void OnEnable()
        {
            health.OnDeath += HandleDeath;
        }
        
        private void HandleDeath()
        {
            AudioManager.Instance?.PlayDeath();
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            controller.enabled = false;
            animator?.SetTrigger(Death);
            if (deathSpritePrefab != null)
                _activeGhost = Instantiate(deathSpritePrefab, controller.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(deathDelay);
            Destroy(_activeGhost);
            controller.transform.position = activeSpawnPoint.transform.position;
            health.ResetHealth();
            animator?.SetTrigger(Respawn);
            controller.enabled = true;
        }
    }
}