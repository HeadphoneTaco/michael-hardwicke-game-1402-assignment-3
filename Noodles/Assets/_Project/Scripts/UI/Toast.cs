using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Displays short toast-style UI messages and exposes a global access point via a singleton instance.
    /// </summary>
    public class Toast : MonoBehaviour
    {
        public static Toast Instance;
        [SerializeField] private GameObject toastUI;
        [SerializeField] private TMP_Text toastText;

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            Instance = this;
        }

        private void Start()
        {
            toastUI.SetActive(false);
        }

        public void ShowToast(string textValue)
        {
            toastUI.SetActive(true);
            toastText.SetText(textValue);
        }

        public void HideToast()
        {
            toastUI.SetActive(false);
        }
    }
}