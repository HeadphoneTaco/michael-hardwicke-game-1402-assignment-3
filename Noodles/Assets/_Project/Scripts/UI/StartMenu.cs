using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Controls the start menu UI by wiring button events for starting or quitting the game.
    /// </summary>
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }

        private void OnPlayClicked()
        {
            SceneManager.LoadScene("Main");
        }

        private void OnQuitClicked()
        {
            Application.Quit();
            // Stops play mode in editor since Application.Quit() won't work there
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}