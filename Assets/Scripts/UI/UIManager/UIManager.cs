using Player;
using UnityEngine;

namespace UI.UIManager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        private bool _isUIActive = false;

        public bool IsUIActive()
        {
            return _isUIActive;
        }

        public void ActivateUI()
        {
            _isUIActive = true;
            playerMovement.ProhibitMovement();
        }

        public void DeactivateUI()
        {
            _isUIActive = false;
            playerMovement.AllowMovement();
        }
    }
}
