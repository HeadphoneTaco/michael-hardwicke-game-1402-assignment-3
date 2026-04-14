using Enums;
using Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Systems
{
    /// <summary>
    ///     Listens for player state changes and switches Cinemachine camera priority
    ///     so the appropriate view is active.
    /// </summary>
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera explorecamera;
        [SerializeField] private CinemachineCamera aimCamera;
        [SerializeField] private PlayerController playerController;
        
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
        
        private void OnEnable()
        {
            playerController.OnStateUpdated += SwitchCamera;
        }
        
        private void OnDisable()
        {
            playerController.OnStateUpdated -= SwitchCamera;
        }

        #endregion
    }
}