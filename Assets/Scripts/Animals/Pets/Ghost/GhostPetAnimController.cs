using System.Collections;
using Animals.Pets.globalAnimControllers;
using Animals.Pets.Interfaces;
using UnityEngine;

namespace Animals.Pets.Ghost
{
    public class GhostPetAnimController : PetAnimController, IStateChangeController
    {
        private Coroutine _stateCoroutine;
        
        private enum States
        {
            Happy,
            Indifferent,
            Scaring
        }

        private IEnumerator ChangeStateAfterTime()
        {
            GenerateRandomDelay();
            yield return new WaitForSeconds(DelayDuration);

            States randomState = GetRandomEnumValue<States>();
            animator.SetTrigger(randomState.ToString());

            _stateCoroutine = null;
            
            if (CanChangeState)
                StartChangingStates();
        }

        public void StartChangingStates()
        {
            CanChangeState = true;
            _stateCoroutine ??= StartCoroutine(ChangeStateAfterTime());
        }

        public void StopChangingStates()
        {
            if (_stateCoroutine == null) return;
            CanChangeState = false;
            StopCoroutine(_stateCoroutine);
            _stateCoroutine = null;
        }
    }
}