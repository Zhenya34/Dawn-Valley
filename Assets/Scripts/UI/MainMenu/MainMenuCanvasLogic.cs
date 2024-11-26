using UnityEditor;
using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject infoPanel;
        [SerializeField] private GameObject exitPanel;
        [SerializeField] private string sceneName;

        private const string TelegramUsername = "Zhenyazhnr_dev";

        public void StartGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public void SwitchOffGame()
        {
            EditorApplication.isPlaying = false;
            Application.Quit();
        }

        public void OpenMainSettings()
        {
            settingsPanel.SetActive(true);
        }

        public void CloseMainSettings()
        {
            settingsPanel.SetActive(false);
        }

        public void GameInfoOpen()
        {
            infoPanel.SetActive(true);
        }

        public void GameInfoClose()
        {
            infoPanel.SetActive(false);
        }

        public void OpenExitPanel()
        {
            exitPanel.SetActive(true);
        }

        public void CloseExitPanel()
        {
            exitPanel.SetActive(false);
        }

        public void OpenOwnTelegram()
        {
            try
            {
                Application.OpenURL($"https://t.me/{TelegramUsername}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to open Telegram. {ex.Message}");
            }
        }
    }
}
