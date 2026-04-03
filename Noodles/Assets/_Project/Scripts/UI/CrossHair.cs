using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Controls crosshair visibility based on the player's current state.
    /// </summary>
    public class CrossHair : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        [SerializeField] private Canvas crossHairCanvas;

        private void Start()
        {
            crossHairCanvas.enabled = false;
        }

        private void OnEnable()
        {
            player.OnStateUpdated += StateUpdate;
        }

        private void OnDisable()
        {
            player.OnStateUpdated -= StateUpdate;
        }

        private void StateUpdate(PlayerState state)
        {
            crossHairCanvas.enabled = state == PlayerState.Aim;
        }
    }
}