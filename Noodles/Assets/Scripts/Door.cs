using UnityEngine;

/// <summary>
///     Controls an animated door and toggles it between open and closed states.
/// </summary>
public class Door : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("_isOpen");
    [SerializeField] private Animator doorAnimator;
    private bool _isOpen;
    
    public void Toggle()
    {
        _isOpen = !_isOpen;
        doorAnimator.SetBool(IsOpen, _isOpen);
    }
}