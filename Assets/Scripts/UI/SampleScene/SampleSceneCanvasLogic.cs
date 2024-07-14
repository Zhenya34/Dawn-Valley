using UnityEngine;

public class SampleSceneCanvasLogic : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _statsPanel;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private string _sceneName;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
    }

    public void OpenPausePanel()
    {
        _pausePanel.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void ClosePausePanel()
    {
        _pausePanel.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void OpenSettingsPanel()
    {
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void OpenStatsPanel()
    {
        _statsPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void CloseStatsPanel()
    {
        _statsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }

    public void OpenInventory()
    {
        _inventoryPanel.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void CloseInventory()
    {
        _inventoryPanel.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void OpenMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneName);
    }
}
