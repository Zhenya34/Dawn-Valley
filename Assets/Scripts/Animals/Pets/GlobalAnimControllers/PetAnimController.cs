using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animals.Pets.globalAnimControllers
{
    public class PetAnimController : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected SpriteRenderer spriteRenderer;
        [SerializeField] private bool hasRunningAnimation;
        [SerializeField] private float minSecondValue = 10f;
        [SerializeField] private float maxSecondValue = 20f;
        
        protected bool CanChangeState;
        protected float DelayDuration;

        private enum Variables
        {
            Running,
            Sleeping
        }

        private void Start() => GenerateRandomDelay();

        protected void GenerateRandomDelay() => DelayDuration = Random.Range(minSecondValue, maxSecondValue);
        
        protected T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Random.Range(0, values.Length));
        }
        
        public void SetRunningAnimation()
        {
            animator.SetBool(Variables.Sleeping.ToString(), false);
            if (hasRunningAnimation)
                animator.SetBool(Variables.Running.ToString(), true);
        }

        public void ActivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), true);
            if (hasRunningAnimation)
                animator.SetBool(Variables.Running.ToString(), false);
        }

        public void DeactivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), false);
            if (hasRunningAnimation)
                animator.SetBool(Variables.Running.ToString(), false);
        }
    }
}