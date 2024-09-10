using System.Collections;
using UnityEngine;

public class Boat_Controller : MonoBehaviour
{
    [SerializeField] private Transform _secondBoatPosition;
    [SerializeField] private Transform _firstBoatPosition;
    [SerializeField] private float _teleportTime;
    [SerializeField] private Transform _player;
    [SerializeField] private bool itFirstBoat;

    static private bool _hasTeleported = false;

    private enum Tags
    {
        Player
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Player.ToString()))
        {
            if (itFirstBoat && _hasTeleported == false)
            {
                StartCoroutine(TeleportToNextBoat(_secondBoatPosition));
            }
            else if (!itFirstBoat && _hasTeleported == false)
            {
                StartCoroutine(TeleportToNextBoat(_firstBoatPosition));
            }
        }
    }

    public void ResetHasTeleported()
    {
        _hasTeleported = false;
        StopAllCoroutines();
    }

    private IEnumerator TeleportToNextBoat(Transform _boatPosition)
    {
        yield return new WaitForSeconds(_teleportTime);
        _player.position = _boatPosition.position;
        _hasTeleported = true;
    }
}