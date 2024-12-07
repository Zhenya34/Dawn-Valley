using System.Collections;
using Animals.Pets.globalAnimControllers;
using UnityEngine;
using IStateChangeController = Animals.Pets.Interfaces.IStateChangeController;

namespace Animals.Pets.CrawlingPets
{
    public class CrawlingPetAnimController : PetAnimController, IStateChangeController
    {
        private Coroutine _stateCoroutine;
        
        private enum States
        {
            Happy,
            Embarrassed,
            Indifferent
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
            if (_stateCoroutine == null)
                _stateCoroutine = StartCoroutine(ChangeStateAfterTime());
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