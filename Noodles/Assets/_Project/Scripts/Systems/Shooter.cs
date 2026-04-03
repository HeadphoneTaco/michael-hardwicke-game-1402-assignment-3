using _Project.Scripts.Enums;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    /// <summary>
    ///     Handles projectile firing based on player input and state.
    ///     Computes a direction from <see cref="shootPoint" /> toward <see cref="aimTrack" />,
    ///     spawns a projectile prefab, applies impulse force, and plays the shoot sound.
    /// </summary>
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private InputAction shootInput;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Transform aimTrack;
        [SerializeField] private GameObject shootObject;
        [SerializeField] private float shootForce;
        private PlayerState _currentState;
        private PlayerController _playerController;
        private GameObject _projectile;
        private Vector3 _shootDirection;

        private void Awake()
        {
            _playerController = GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            shootInput.Enable();
            shootInput.performed += Shoot;
            _playerController.OnStateUpdated += StateUpdate;
        }

        private void OnDisable()
        {
            shootInput.performed -= Shoot;
            _playerController.OnStateUpdated -= StateUpdate;
        }

        private void StateUpdate(PlayerState state)
        {
            _currentState = state;
        }

        private void Shoot(InputAction.CallbackContext context)
        {
            if (_currentState != PlayerState.Aim) return;
            _shootDirection = aimTrack.position - shootPoint.position;
            _shootDirection.Normalize();
            _projectile = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(_shootDirection));
            _projectile.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
            AudioManager.Instance?.PlayShoot();
        }
    }
}