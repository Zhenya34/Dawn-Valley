using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionDoor : MonoBehaviour
{
    [SerializeField] private float _teleportDelay;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private string _externalSceneName;
    [SerializeField] private string[] _internalSceneNames;
    [SerializeField] private bool _isInternalDoor;
    [SerializeField] private string _targetScene;

    private bool _isTeleporting = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isTeleporting)
        {
            _isTeleporting = true;
            StartCoroutine(DelayBeforeTeleporting());
        }
    }

    private IEnumerator DelayBeforeTeleporting()
    {
        yield return new WaitForSeconds(_teleportDelay);
        if (_isInternalDoor)
        {
            SceneManager.LoadScene(_externalSceneName);
        }
        else
        {
            _targetScene = _internalSceneNames[Random.Range(0, _internalSceneNames.Length)];
            SceneManager.LoadScene(_targetScene);
        }
        _isTeleporting = false;
    }
}
