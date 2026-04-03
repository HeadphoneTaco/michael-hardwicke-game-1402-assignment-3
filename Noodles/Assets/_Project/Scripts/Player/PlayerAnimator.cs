using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Synchronizes player movement state with animator parameters and triggers jump animations.
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Animator anim;
        private Vector3 _playerVelocity;

        private void Update()
        {
            anim.SetBool("IsGrounded", playerController.IsGrounded());

            _playerVelocity = playerController.GetPlayerVelocity();
            _playerVelocity.y = 0;

            anim.SetFloat("Velocity", _playerVelocity.sqrMagnitude);
        }

        private void OnEnable()
        {
            playerController.OnJumpEvent += OnJump;
        }

        private void OnDisable()
        {
            playerController.OnJumpEvent -= OnJump;
        }

        private void OnJump()
        {
            anim.SetTrigger("Jump");
        }
    }
}