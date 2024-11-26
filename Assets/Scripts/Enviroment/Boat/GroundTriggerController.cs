using UnityEngine;

namespace Enviroment.Boat
{
    public class GroundTriggerController : MonoBehaviour
    {
        [SerializeField] private BoatController boatController;
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            boatController.ResetHasTeleported();
        }
    }
}
