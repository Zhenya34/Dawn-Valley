using System.Collections;
using UnityEngine;

public class Boat_Controller : MonoBehaviour
{
    [SerializeField] private Transform _secondBoatPosition;
    [SerializeField] private Transform _firstBoatPosition;
    [SerializeField] private float _teleportTime;
    [SerializeField] private Transform _player;

    private bool _isTeleporting = false;
    private bool _hasTeleported = false;
    static private bool _isOnFirstBoat = true;

    private enum Tags
    {
        Player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player.ToString()) && !_hasTeleported && !_isTeleporting)
        {
            _isTeleporting = true;
            StartCoroutine(BoatTeleporting());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player.ToString()))
        {
            _isTeleporting = false;
            _hasTeleported = false;
        }
    }

    private IEnumerator BoatTeleporting()
    {
        yield return new WaitForSeconds(_teleportTime);

        if (_isTeleporting)
        {
            if (!_hasTeleported)
            {
                if (_isOnFirstBoat)
                {
                    _player.position = _secondBoatPosition.position;
                    _isOnFirstBoat = false;
                }
                else
                {
                    _player.position = _firstBoatPosition.position;
                    _isOnFirstBoat = true;
                }

                _hasTeleported = true;
            }
            _isTeleporting = false;
        }
    }
}