using DG.Tweening;
using Interfaces;
using UI;
using UnityEngine;

/// <summary>
///     Interactable chest that plays open/close animation,
///     and displays a contextual interaction toast.
/// </summary>
public class ChestInteractable : MonoBehaviour, IInteractable
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    [SerializeField] private Animator anim;
    private Tween collectTween;
    private Tween loopTween;
    
    private void Start()
    {
        if (!anim) return;

        transform.DOScale(1.2f, .5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
    
    private void OnDestroy()
    {
        transform.DOKill();
    }
    
    public void OnHoverIn()
    {
        if (anim != null)
        {
            anim?.SetBool(IsOpen, true);
            Toast.Instance.ShowToast("Press \"E\" to Interact");
        }
    }

    public void OnHoverOff()
    {
        if (anim != null)
        {
            anim?.SetBool(IsOpen, false);
            Toast.Instance.HideToast();
        }
    }
    
    public void OnInteract()
    {
        if (anim != null)
            collectTween = transform.DOScale(0, .5f).SetEase(Ease.InBack)
                .OnComplete(() => { Destroy(gameObject); });
    }
}