using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Controls the win screen UI and routes button actions to the <see cref="GameManager" />.
    /// </summary>
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            GameManager.Instance.OnGameWin += Show;
            Hide();
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameWin -= Show;
        }

        private void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        private void OnRestartClicked()
        {
            GameManager.Instance.RestartGame();
        }

        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}