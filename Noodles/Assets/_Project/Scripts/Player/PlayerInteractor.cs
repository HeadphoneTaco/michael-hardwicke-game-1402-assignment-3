using _Project.Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts
{
    /// <summary>
    ///     Manages player interactions with interactable objects in the game world.
    ///     Detects when the player enters/exits trigger zones and routes interaction input
    ///     to objects that implement the <see cref="IInteractable" /> interface.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private InputAction interactionInput;
        private IInteractable _interactable;
        private IInteractable _tempInteractable;

        private void OnEnable()
        {
            interactionInput.Enable();
            interactionInput.performed += Interact;
        }

        //TODO:Make these enable/disable matching pattern.
        private void OnDisable()
        {
            interactionInput.performed -= Interact;
        }

        private void OnTriggerEnter(Collider other)
        {
            _tempInteractable = other.GetComponent<IInteractable>();

            if (_tempInteractable != null)
            {
                _interactable = _tempInteractable;
                _interactable?.OnHoverIn();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _interactable?.OnHoverOff();
            _interactable = null;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            _interactable?.OnInteract();
        }
    }
}