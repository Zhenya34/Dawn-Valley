using System.Collections;
using UnityEngine;

public class BeePetAnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sr;

    private readonly float _minSecondValue = 16f;
    private readonly float _maxSecondValue = 22f;
    private float _middleSecondValue;

    private enum Triggers
    {
        coup,
        coupFliped
    }

    private enum Variables
    {
        sleeping
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

        if(_sr.flipX == false)
        {
            _animator.SetTrigger(Triggers.coup.ToString());
        }
        else
        {
            _animator.SetTrigger(Triggers.coupFliped.ToString());
        }
        GenerateRandomDelay();
        StartCoroutine(TriggerFlipAfterTime(_middleSecondValue));
    }

    public void ActivateNightTime()
    {
        _animator.SetBool(Variables.sleeping.ToString(), true);
    }

    public void DeactivateNightTime()
    {
        _animator.SetBool(Variables.sleeping.ToString(), false);
    }
}
