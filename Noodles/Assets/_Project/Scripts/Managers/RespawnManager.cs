using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    /// <summary>
    ///     Handles player death and respawn flow, including death effects, delay, and repositioning
    ///     to the currently active spawn point.
    /// </summary>
    public class RespawnManager : MonoBehaviour
    {
        private static RespawnManager _instance;
        private static readonly int Respawn = Animator.StringToHash("Respawn");
        private static readonly int Death = Animator.StringToHash("Death");
        [SerializeField] private Health playerHealth;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private SpawnPoint activeSpawnPoint;
        [SerializeField] private GameObject ghostPrefab;
        [Space(10)] [SerializeField] private float deathDelay = 2f;
        private GameObject activeGhost;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        private void OnEnable()
        {
            playerHealth.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            playerHealth.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            AudioManager.Instance?.PlayDeath();
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            playerController.enabled = false;
            if (playerAnimator != null) playerAnimator.SetTrigger(Death);
            if (ghostPrefab != null)
                activeGhost = Instantiate(ghostPrefab, playerController.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(deathDelay);
            if (activeGhost != null) Destroy(activeGhost);
            playerController.transform.position = activeSpawnPoint.transform.position;
            playerHealth.ResetHealth();
            playerAnimator.SetTrigger(Respawn);
            playerController.enabled = true;
        }

        public void SetSpawnPoint(SpawnPoint newSpawnPoint)
        {
            activeSpawnPoint = newSpawnPoint;
            Debug.Log($"Checkpoint set: {newSpawnPoint.gameObject.name}");
        }
    }
}