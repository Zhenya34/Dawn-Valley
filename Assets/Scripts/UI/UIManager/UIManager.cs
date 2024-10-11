using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player_Movement _playerMovement;
    private bool _isUIActive = false;

    public bool IsUIActive()
    {
        return _isUIActive;
    }

    public void ActivateUI()
    {
        _isUIActive = true;
        _playerMovement.ProhibitMovement();
    }

    public void DeactivateUI()
    {
        _isUIActive = false;
        _playerMovement.AllowMovement();
    }
}
