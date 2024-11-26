using System;
using System.Collections;
using UnityEngine;
using Animals.Pets.Namespace;

namespace Animals.Pets.globalAnimControllers
{
    public class CrawlingPetAnimController : MonoBehaviour, INightTimeController
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private float minSecondValue;
        [SerializeField] private float maxSecondValue;

        private float _middleSecondValue;
        private States _selectedState;

        private enum States
        {
            Happy,
            Embarrassed,
            Indifferent
        }

        private enum Variables
        {
            Sleeping
        }

        private void Start()
        {
            GenerateRandomDelay();
            StartCoroutine(ChangeEmotionAfterTime(_middleSecondValue));
        }

        private void GenerateRandomDelay()
        {
            _middleSecondValue = UnityEngine.Random.Range(minSecondValue, maxSecondValue);
        }

        private void GenerateRandomState()
        {
            _selectedState = GetRandomEnumValue<States>();
        }

        private IEnumerator ChangeEmotionAfterTime(float time)
        {
            yield return new WaitForSeconds(time);
            GenerateRandomState();
            animator.SetTrigger(_selectedState.ToString());
            GenerateRandomDelay();
            StartCoroutine(ChangeEmotionAfterTime(_middleSecondValue));
        }

        private T GetRandomEnumValue<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int randomIndex = UnityEngine.Random.Range(0, values.Length);
            return (T)values.GetValue(randomIndex);
        }

        public void ActivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), true);
        }

        public void DeactivateNightTime()
        {
            animator.SetBool(Variables.Sleeping.ToString(), false);
        }
    }
}
