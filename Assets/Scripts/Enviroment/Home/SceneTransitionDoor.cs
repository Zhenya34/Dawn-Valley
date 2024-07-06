using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionDoor : MonoBehaviour
{
    [SerializeField] private float _teleportDelay;
    [SerializeField] private Vector3 _targetPositionExternal;
    [SerializeField] private GameObject _player;
    
    private bool _isInternalDoor;
    private bool _isTeleporting = false;
    private Coroutine _teleportCoroutine = null;

    static private bool _isPreviousSceneHome = false;

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == SceneNames.SampleScene.ToString() && _isPreviousSceneHome == true)
        {
            _player.transform.position = _targetPositionExternal;
            _isPreviousSceneHome = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isTeleporting)
        {
            _isTeleporting = true;
            _isInternalDoor = true;
            _teleportCoroutine = StartCoroutine(DelayBeforeTeleporting());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_teleportCoroutine != null)
        {
            StopCoroutine(_teleportCoroutine);
            _isTeleporting = false;
            _teleportCoroutine = null;
        }
    }

    private enum SceneNames
    {
        SampleScene,
        HomeSceneMini,
        HomeSceneMiddle,
        HomeSceneMax
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isTeleporting)
        {
            _isTeleporting = true;
            _isInternalDoor = false;
            _teleportCoroutine = StartCoroutine(DelayBeforeTeleporting());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_teleportCoroutine != null)
        {
            StopCoroutine(_teleportCoroutine);
            _isTeleporting = false;
            _teleportCoroutine = null;
        }
    }

    private IEnumerator DelayBeforeTeleporting()
    {
        yield return new WaitForSeconds(_teleportDelay);

        if (_isInternalDoor)
        {
            _isPreviousSceneHome = true;
            SceneManager.LoadScene(SceneNames.SampleScene.ToString());
        }
        else
        {
            _isPreviousSceneHome = false;
            SceneManager.LoadScene(SceneNames.HomeSceneMax.ToString());
        }

        _isTeleporting = false;
        _teleportCoroutine = null;
    }
}

