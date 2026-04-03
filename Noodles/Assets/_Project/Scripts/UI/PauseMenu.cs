using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Controls the pause screen UI and routes button actions to the <see cref="GameManager" />.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            GameManager.Instance.OnGamePaused += Show;
            Hide();
        }

        //TODO: Streamline to only have OnGamePaused
        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += Show;
            GameManager.Instance.OnGameResumed += Hide;
        }

        //TODO: Streamline to only have OnGamePaused
        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= Show;
            GameManager.Instance.OnGameResumed -= Hide;
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

        private void OnResumeClicked()
        {
            GameManager.Instance.Resume();
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