using System.Collections;
using Animals.Pets.Namespace;
using UnityEngine;

namespace Animals.Pets.Bee
{
    public class BeePetAnimController : MonoBehaviour, INightTimeController
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer sr;

        private readonly float _minSecondValue = 16f;
        private readonly float _maxSecondValue = 22f;
        private float _middleSecondValue;

        private enum Triggers
        {
            Coup,
            CoupFliped
        }

        private enum Variables
        {
            Sleeping
        }

        private void Start()
        {
            GenerateRandomDelay();
            StartCoroutine(TriggerFlipAfterTime(_middleSecondValue));
        }

        private void GenerateRandomDelay()
        {
            _middleSecondValue = Random.Range(_minSecondValue, _maxSecondValue);
        }

        private IEnumerator TriggerFlipAfterTime(float time)
        {
            yield return new WaitForSeconds(time);

            if(sr.flipX == false)
            {
                animator.SetTrigger(Triggers.Coup.ToString());
            }
            else
            {
                animator.SetTrigger(Triggers.CoupFliped.ToString());
            }

            GenerateRandomDelay();
            StartCoroutine(TriggerFlipAfterTime(_middleSecondValue));
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
