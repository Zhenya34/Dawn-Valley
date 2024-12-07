using System;
using Player;
using UI.SampleScene;
using UnityEngine;

namespace UI.UIManager
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private ToolSwitcher toolSwitcher;
        private bool _isUIActive;

        public bool IsUIActive() => _isUIActive;

        private void Awake()
        {
            if(toolSwitcher)
                toolSwitcher.AllowToolChanges();
        }

        public void ActivateUI()
        {
            _isUIActive = true;
            playerMovement.ProhibitMovement();
            if(toolSwitcher)
                toolSwitcher.ProhibitToolChanges();
        }

        public void DeactivateUI()
        {
            _isUIActive = false;
            playerMovement.AllowMovement();
            if(toolSwitcher)
                toolSwitcher.AllowToolChanges();
        }
    }
}
