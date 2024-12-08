using System.Collections;
using UnityEngine;
using Animals.Pets.globalAnimControllers;
using Animals.Pets.Interfaces;

namespace Animals.Pets.Bee
{
    public class BeePetAnimController : PetAnimController, IStateChangeController
    {
        private Coroutine _flipCoroutine;

        private enum Triggers
        {
            Coup,
            CoupFlipped
        }

        private IEnumerator ChangeStateAfterTime()
        {
            GenerateRandomDelay();
            yield return new WaitForSeconds(DelayDuration);

            animator.SetTrigger(spriteRenderer.flipX ? Triggers.CoupFlipped.ToString() : Triggers.Coup.ToString());

            _flipCoroutine = null;

            if (CanChangeState)
                StartChangingStates();
        }
        
        public void StartChangingStates()
        {
            CanChangeState = true;
            _flipCoroutine ??= StartCoroutine(ChangeStateAfterTime());
        }

        public void StopChangingStates()
        {
            if (_flipCoroutine == null) return;
            CanChangeState = false;
            StopCoroutine(_flipCoroutine);
            _flipCoroutine = null;
        }
    }
}