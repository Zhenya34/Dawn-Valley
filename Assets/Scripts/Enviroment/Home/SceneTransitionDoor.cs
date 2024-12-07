using System.Collections;
using UI.SampleScene.Upgrades;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enviroment.Home
{
    public class SceneTransitionDoor : MonoBehaviour
    {
        [SerializeField] private float teleportDelay;
        [SerializeField] private Vector3 targetPositionExternal;
        [SerializeField] private GameObject player;
        [SerializeField] private HouseLevelManager houseLevelManager;
    
        private bool _isInternalDoor;
        private bool _isTeleporting;
        private Coroutine _teleportCoroutine;

        private static bool _isPreviousSceneHome;

        private void Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;

            if (currentSceneName == HouseLevelManager.SceneNames.SampleScene.ToString() && _isPreviousSceneHome)
            {
                player.transform.position = targetPositionExternal;
                _isPreviousSceneHome = false;
            }
        }

        private void OnCollisionEnter2D()
        {
            if (!_isTeleporting)
            {
                _isTeleporting = true;
                _isInternalDoor = true;
                _teleportCoroutine = StartCoroutine(DelayBeforeTeleporting());
            }
        }

        private void OnCollisionExit2D()
        {
            if (_teleportCoroutine != null)
            {
                StopCoroutine(_teleportCoroutine);
                _isTeleporting = false;
                _teleportCoroutine = null;
            }
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
            yield return new WaitForSeconds(teleportDelay);

            if (_isInternalDoor)
            {
                _isPreviousSceneHome = true;
                SceneManager.LoadScene(HouseLevelManager.SceneNames.SampleScene.ToString());
            }
            else
            {
                _isPreviousSceneHome = false;
                SceneManager.LoadScene(houseLevelManager.GetSceneForHouseLevel().ToString());
            }

            _isTeleporting = false;
            _teleportCoroutine = null;
        }
    }
}