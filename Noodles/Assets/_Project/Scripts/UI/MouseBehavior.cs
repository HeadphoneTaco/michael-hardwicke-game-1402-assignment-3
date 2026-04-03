using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Controls cursor visibility and lock state for gameplay and UI interactions.
    /// </summary>
    public class MouseBehavior : MonoBehaviour
    {
        private void Start()
        {
            ShowMouse(false);
        }

        public void ShowMouse(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}