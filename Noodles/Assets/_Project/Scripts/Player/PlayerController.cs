using System;
using _Project.Scripts.Enums;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    /// <summary>
    ///     Manages player character movement, aiming, jumping, and state transitions.
    ///     Supports two distinct movement modes: Explore and Aim.
    ///     Handles input processing, velocity calculations, gravity, and ground detection.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float moveSpeed = 30;
        [SerializeField] private float accelerationSpeed = 20f;
        [SerializeField] private float decelerationSpeed = 25f;
        [SerializeField] private float rotationSpeed = 10;
        [SerializeField] public float gravity = -9.8f;
        [SerializeField] private float jumpVelocity = 10f;
        [SerializeField] private float moveSpeedAimed = 2;
        [SerializeField] private float rotationSpeedAimed = 2;
        [SerializeField] private Transform aimTrack;
        [SerializeField] private float maxaimHeight;
        [SerializeField] private float minaimHeight;
        [SerializeField] private Vector3 groundCheckOffset;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckRadius;
        [SerializeField] private LayerMask groundLayer;
        private Vector3 _camForward;
        private Vector3 _camRight;
        private CharacterController _characterController;
        private Vector3 _currentHorizontalVelocity;
        private PlayerState _currentState;
        private Vector3 _defaultAimTrackerPosition;
        private bool _isGrounded;
        private Vector2 _lookInput;
        private Vector3 _moveDirection;
        private Vector2 _moveInput;
        private Quaternion _targetRotation;
        private Vector3 _tempAimTrackerPosition;
        private Vector3 _velocity;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
            Gizmos.DrawSphere(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance,
                groundCheckRadius);
            Gizmos.DrawCube(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance / 2,
                new Vector3(1.5f * groundCheckRadius, groundCheckDistance, 1.5f * groundCheckRadius));
        }

        public event Action OnJumpEvent;
        public event Action<PlayerState> OnStateUpdated;

        public bool IsGrounded()
        {
            return _isGrounded;
        }

        public Vector3 GetPlayerVelocity()
        {
            return _velocity;
        }

        public void DisableInput()
        {
            enabled = false;
        }

        #region Unity Functions

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();

            //Set default state
            _currentState = PlayerState.Explore;
            OnStateUpdated?.Invoke(_currentState);

            _defaultAimTrackerPosition = aimTrack.localPosition;
        }

        private void Update()
        {
            if (_currentState == PlayerState.Explore)
            {
                CalculateMovementExplore();
                aimTrack.localPosition = _defaultAimTrackerPosition;
            }
            else if (_currentState == PlayerState.Aim)
            {
                CalculateMovementAim();
                UpdateAimTrack();
            }


            _characterController.Move(_velocity * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            CheckGrounded();
            if (_isGrounded && _velocity.y < 0) _velocity.y = -0.2f;
        }

        #endregion

        #region Movement and Aiming

        public void OnMove(InputValue value)
        {
            //Guard against non-playing states
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            _moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            _lookInput = value.Get<Vector2>();
        }

        public void OnJump()
        {
            //Guard against non-playing states
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            if (_isGrounded)
            {
                _velocity.y = jumpVelocity;
                OnJumpEvent?.Invoke();
            }
        }

        public void OnAim(InputValue value)
        {
            //Guard against non-playing states
            if (GameManager.Instance.CurrentState != GameState.Playing) return;

            _currentState = value.isPressed ? PlayerState.Aim : PlayerState.Explore;
            OnStateUpdated?.Invoke(_currentState);

            if (_currentState == PlayerState.Aim)
            {
                _camForward = playerCamera.transform.forward;
                _camForward.y = 0;
                _camForward.Normalize();
                transform.rotation = Quaternion.LookRotation(_camForward);
            }
        }

        private void CalculateMovementExplore()
        {
            _camForward = playerCamera.transform.forward;
            _camRight = playerCamera.transform.right;
            _camForward.y = 0;
            _camRight.y = 0;
            _camForward.Normalize();
            _camRight.Normalize();

            _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

            if (_moveDirection.sqrMagnitude > 0.01f)
            {
                _targetRotation = Quaternion.LookRotation(_moveDirection);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
            }

            var targetHorizontalVelocity = _moveDirection * moveSpeed;

            var rate = _moveDirection.sqrMagnitude > 0.01f ? accelerationSpeed : decelerationSpeed;
            _currentHorizontalVelocity = Vector3.MoveTowards(
                _currentHorizontalVelocity,
                targetHorizontalVelocity,
                rate * Time.deltaTime
            );
            _velocity = _currentHorizontalVelocity + Vector3.up * _velocity.y;
            _velocity.y += gravity * Time.deltaTime;
        }

        //TODO:Move Explore and Aim to separate namespace for movement. Swimming and Climbing should be considered.
        private void CalculateMovementAim()
        {
            transform.Rotate(Vector3.up, rotationSpeed * _lookInput.x * Time.deltaTime);
            _moveDirection = _moveInput.x * transform.right + _moveInput.y * transform.forward;
            _velocity = _velocity.y * Vector3.up + moveSpeedAimed * _moveDirection;
            _velocity.y += gravity * Time.deltaTime;
        }

        private void UpdateAimTrack()
        {
            _tempAimTrackerPosition = aimTrack.localPosition;
            _tempAimTrackerPosition.y += _lookInput.y * rotationSpeedAimed * Time.deltaTime;
            _tempAimTrackerPosition.y = Mathf.Clamp(_tempAimTrackerPosition.y, minaimHeight, maxaimHeight);

            aimTrack.localPosition = _tempAimTrackerPosition;
        }

        private void CheckGrounded()
        {
            _isGrounded = Physics.SphereCast(
                transform.position + groundCheckOffset,
                groundCheckRadius,
                Vector3.down,
                out var hit,
                groundCheckDistance,
                groundLayer
            );
        }

        #endregion
    }
}