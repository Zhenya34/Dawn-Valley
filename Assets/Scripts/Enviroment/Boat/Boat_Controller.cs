using System.Collections;
using UnityEngine;

namespace Enviroment.Boat
{
    public class BoatController : MonoBehaviour
    {
        [SerializeField] private Transform secondBoatPosition;
        [SerializeField] private Transform firstBoatPosition;
        [SerializeField] private float teleportTime;
        [SerializeField] private Transform player;
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
                    StartCoroutine(TeleportToNextBoat(secondBoatPosition));
                }
                else if (!itFirstBoat && _hasTeleported == false)
                {
                    StartCoroutine(TeleportToNextBoat(firstBoatPosition));
                }
            }
        }

        public void ResetHasTeleported()
        {
            _hasTeleported = false;
            StopAllCoroutines();
        }

        private IEnumerator TeleportToNextBoat(Transform boatPosition)
        {
            yield return new WaitForSeconds(teleportTime);
            player.position = boatPosition.position;
            _hasTeleported = true;
        }
    }
}