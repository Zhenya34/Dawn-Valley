using UnityEngine;
using UnityEngine.SceneManagement;

namespace Saves
{
    public class GameSaver : MonoBehaviour
    {
        public void SaveSettings(float soundVolume, float musicVolume, bool fullscreenMode, string currentLanguage, bool vSyncEnabled, int targetFPS, float cameraZoom, bool targetingMarkerEnabled)
        {
            PlayerPrefs.SetFloat("SoundVolume", soundVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetInt("FullscreenMode", fullscreenMode ? 1 : 0);
            PlayerPrefs.SetString("CurrentLanguage", currentLanguage);
            PlayerPrefs.SetInt("VSyncEnabled", vSyncEnabled ? 1 : 0);
            PlayerPrefs.SetInt("TargetFPS", targetFPS);
            PlayerPrefs.SetFloat("CameraZoom", cameraZoom);
            PlayerPrefs.SetInt("TargetingMarkerEnabled", targetingMarkerEnabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void LoadSettings(out float soundVolume, out float musicVolume, out bool fullscreenMode, out string currentLanguage, out bool vSyncEnabled, out int targetFPS, out float cameraZoom, out bool targetingMarkerEnabled)
        {
            soundVolume = PlayerPrefs.GetFloat("SoundVolume", 1.0f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            fullscreenMode = PlayerPrefs.GetInt("FullscreenMode", 0) == 1;
            currentLanguage = PlayerPrefs.GetString("CurrentLanguage", "Русский");
            vSyncEnabled = PlayerPrefs.GetInt("VSyncEnabled", 0) == 1;
            targetFPS = PlayerPrefs.GetInt("TargetFPS", 60);
            cameraZoom = PlayerPrefs.GetFloat("CameraZoom", 3.55f);
            targetingMarkerEnabled = PlayerPrefs.GetInt("TargetingMarkerEnabled", 1) == 1;
        }

        public void SaveGameProgress()
        {
            PlayerPrefs.SetInt("GameProgress", 1);
            PlayerPrefs.Save();
        }

        public void DeleteAllSaves()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}