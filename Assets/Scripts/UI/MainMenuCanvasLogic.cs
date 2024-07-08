using UnityEditor;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private GameObject _exitPanel;
    [SerializeField] private string _sceneName;

    private readonly string _telegramUsername = "Zhenyazhnr_dev";

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }

    public void SwitchOffGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void OpenMainSettings()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseMainSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void GameInfoOpen()
    {
        _infoPanel.SetActive(true);
    }

    public void GameInfoClose()
    {
        _infoPanel.SetActive(false);
    }

    public void OpenExitPanel()
    {
        _exitPanel.SetActive(true);
    }

    public void CloseExitPanel()
    {
        _exitPanel.SetActive(false);
    }

    public void OpenOwnTelegram()
    {
        try
        {
            Application.OpenURL($"https://t.me/{_telegramUsername}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to open Telegram. {ex.Message}");
        }
    }
}
