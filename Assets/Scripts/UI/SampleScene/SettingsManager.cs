using Saves;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SampleScene
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private GameSaver gameSaver;
        [SerializeField] private GameObject targetIndicator;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private Text languageText;

        private float _soundVolume = 1.0f;
        private float _musicVolume = 1.0f;
        private float _cameraZoom = 3.55f;
        private bool _fullscreenMode;
        private string _currentLanguage = "Русский";
        private bool _vSyncEnabled;
        private int _targetFPS = 60;

        public void SetSoundVolume(float volume)
        {
            _soundVolume = Mathf.Clamp(volume, 0f, 1f);
            audioSource.volume = _soundVolume;
        }

        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp(volume, 0f, 1f);
            audioSource.volume = _soundVolume;
        }

        public void SaveGame()
        {
            gameSaver.SaveSettings(_soundVolume, _musicVolume, _fullscreenMode, _currentLanguage, _vSyncEnabled, _targetFPS, _cameraZoom, targetIndicator.activeSelf);
            gameSaver.SaveGameProgress();
        }

        public void DeleteAllSaves()
        {
            gameSaver.DeleteAllSaves();
        }

        public void SetCameraZoom(float zoomLevel)
        {
            _cameraZoom = Mathf.Clamp(zoomLevel, 3f, 4f);
            playerCamera.orthographicSize = _cameraZoom;
        }

        public void ToggleTargetingMarker(bool isEnabled)
        {
            targetIndicator.SetActive(isEnabled);
        }

        public void ToggleFullscreenMode(bool isEnabled)
        {
            _fullscreenMode = isEnabled;
            Screen.fullScreen = _fullscreenMode;
        }

        public void CycleLanguage()
        {
            _currentLanguage = _currentLanguage == "Русский" ? "English" : "Русский";
            languageText.text = _currentLanguage;
        }

        public void ToggleVSync(bool isEnabled)
        {
            _vSyncEnabled = isEnabled;
            QualitySettings.vSyncCount = _vSyncEnabled ? 1 : 0;
        }

        public void ChangeGraphicsQuality(int qualityLevel)
        {
            qualityLevel = Mathf.Clamp(qualityLevel, 0, 2);
            QualitySettings.SetQualityLevel(qualityLevel, true);
        }

        public void ChangeTargetFPS(bool isRightArrow)
        {
            if (isRightArrow)
            {
                if (_targetFPS == 60) _targetFPS = 75;
                else if (_targetFPS == 75) _targetFPS = 100;
                else if (_targetFPS == 100) _targetFPS = 120;
            }
            else
            {
                if (_targetFPS == 60) _targetFPS = 45;
                else if (_targetFPS == 45) _targetFPS = 30;
            }

            Application.targetFrameRate = _targetFPS;
        }
    }
}