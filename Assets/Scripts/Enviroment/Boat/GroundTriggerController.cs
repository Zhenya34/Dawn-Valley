using UnityEngine;

public class GroundTriggerController : MonoBehaviour
{
    [SerializeField] private Boat_Controller _boatController;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _boatController.ResetHasTeleported();
    }
}
