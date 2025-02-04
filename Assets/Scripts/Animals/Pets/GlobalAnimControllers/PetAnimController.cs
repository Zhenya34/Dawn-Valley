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
        
        private void SetAnimatorBools(bool isSleeping, bool isRunning)
        {
            animator.SetBool(Variables.Sleeping.ToString(), isSleeping);
            if (hasRunningAnimation)
                animator.SetBool(Variables.Running.ToString(), isRunning);
        }

        public void SetRunningAnimation() => SetAnimatorBools(false, true);

        public void ActivateNightTime() => SetAnimatorBools(true, false);

        public void DeactivateNightTime() => SetAnimatorBools(false, false);
    }
}