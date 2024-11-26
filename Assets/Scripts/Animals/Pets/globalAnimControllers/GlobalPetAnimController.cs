using UnityEngine;
using Animals.Pets.Namespace;

namespace Animals.Pets.globalAnimControllers
{
    public class GlobalPetAnimController : MonoBehaviour, INightTimeController
    {
        [SerializeField] private Animator animator;

        private enum Variables
        {
            Running,
            Sleeping
        }

        public void SetRunningAnimation()
        {
            animator.SetBool(Variables.Sleeping.ToString(), false);
            animator.SetBool(Variables.Running.ToString(), true);
        }

        public void ActivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), true);
            animator.SetBool(Variables.Running.ToString(), false);
        }

        public void DeactivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), false);
            animator.SetBool(Variables.Running.ToString(), false);
        }
    }
}
