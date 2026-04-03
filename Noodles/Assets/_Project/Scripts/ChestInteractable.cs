using _Project.Scripts.Interfaces;
using _Project.Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    /// <summary>
    ///     Interactable chest that plays open/close animation,
    ///     and displays a contextual interaction toast.
    /// </summary>
    public class ChestInteractable : MonoBehaviour, IInteractable
    {
        /// <summary>
        ///     Cached animator parameter hash for the <c>IsOpen</c> bool.
        /// </summary>
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        /// <summary>
        ///     Animator controlling chest open/close state.
        /// </summary>
        [SerializeField] private Animator anim;

        /// <summary>
        ///     Tween handle for the collect animation before destroying the chest.
        /// </summary>
        //This field 'collectTween' IS being used, Intellisense is being silly
        private Tween collectTween;

        /// <summary>
        ///     Tween handle intended for the idle looping scale animation.
        /// </summary>
        private Tween loopTween;

        /// <summary>
        ///     Starts the idle pulse animation if an animator reference is available.
        /// </summary>
        private void Start()
        {
            if (!anim) return;

            transform.DOScale(1.2f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
        }

        /// <summary>
        ///     Cleans up any active DOTweens associated with this transform
        ///     when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            transform.DOKill();
        }

        /// <summary>
        ///     Called when the player starts hovering this chest.
        ///     Opens the chest animation and shows toast.
        /// </summary>
        public void OnHoverIn()
        {
            if (anim != null)
            {
                anim?.SetBool(IsOpen, true);
                Toast.Instance.ShowToast("Press \"E\" to Interact");
            }
        }

        /// <summary>
        ///     Called when the player stops hovering this chest.
        ///     Closes the chest animation and hides toast.
        /// </summary>
        public void OnHoverOff()
        {
            if (anim != null)
            {
                anim?.SetBool(IsOpen, false);
                Toast.Instance.HideToast();
            }
        }

        /// <summary>
        ///     Called when the player interacts with this chest.
        ///     Plays a scale-down tween and destroys the object on completion.
        /// </summary>
        public void OnInteract()
        {
            if (anim != null)
                collectTween = transform.DOScale(0, .5f).SetEase(Ease.InBack)
                    .OnComplete(() => { Destroy(gameObject); });
        }
    }
}