using _Project.Scripts.Enums;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Listens for player state changes and switches Cinemachine camera priority
    ///     so the appropriate view is active.
    /// </summary>
    public class CameraSwitcher : MonoBehaviour
    {
        /// <summary>
        ///     Camera used during normal exploration.
        /// </summary>
        [SerializeField] private CinemachineCamera explorecamera;

        /// <summary>
        ///     Camera used while the player is aiming.
        /// </summary>
        [SerializeField] private CinemachineCamera aimCamera;

        /// <summary>
        ///     Player controller that publishes state updates.
        /// </summary>
        [SerializeField] private PlayerController playerController;

        /// <summary>
        ///     Switches to the camera that matches the provided player state by
        ///     prioritizing the corresponding Cinemachine camera.
        /// </summary>
        /// <param name="state">The latest player state.</param>
        private void SwitchCamera(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Explore:
                    explorecamera.Prioritize();
                    break;

                case PlayerState.Aim:
                    aimCamera.Prioritize();
                    break;

                // ReSharper disable once RedundantEmptySwitchSection
                default:
                    // No camera change required for unsupported states.
                    break;
            }
        }

        #region Unity Functions

        /// <summary>
        ///     Subscribes to player state updates when this component becomes active.
        /// </summary>
        private void OnEnable()
        {
            playerController.OnStateUpdated += SwitchCamera;
        }

        /// <summary>
        ///     Unsubscribes from player state updates when this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            playerController.OnStateUpdated -= SwitchCamera;
        }

        #endregion
    }
}