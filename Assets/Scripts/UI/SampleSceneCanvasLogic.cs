using UnityEngine;

public class SampleSceneCanvasLogic : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private string _sceneName;

    public void OpenPausePanel()
    {
        _pausePanel.SetActive(true);
    }

    public void ClosePausePanel()
    {
        _pausePanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenStatsPanel()
    {
        _statsPanel.SetActive(true);
    }

    public void CloseStatsPanel()
    {
        _statsPanel.SetActive(false);
    }

    public void OpenMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}
