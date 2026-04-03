using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Controls an animated door and toggles it between open and closed states.
    /// </summary>
    public class Door : MonoBehaviour
    {
        /// <summary>
        ///     Cached animator parameter hash for the <c>_isOpen</c> boolean.
        /// </summary>
        private static readonly int IsOpen = Animator.StringToHash("_isOpen");

        /// <summary>
        ///     Animator that drives the door's open/close animation states.
        /// </summary>
        [SerializeField] private Animator doorAnimator;

        /// <summary>
        ///     Tracks whether the door is currently considered open.
        /// </summary>
        private bool _isOpen;

        /// <summary>
        ///     Flips the door state and updates the animator parameter to match.
        /// </summary>
        public void Toggle()
        {
            _isOpen = !_isOpen;
            doorAnimator.SetBool(IsOpen, _isOpen);
        }
    }
}