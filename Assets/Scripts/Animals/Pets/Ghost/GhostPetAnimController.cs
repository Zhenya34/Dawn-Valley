using System;
using System.Collections;
using UnityEngine;

public class GhostPetAnimController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private float _minSecondValue;
    [SerializeField] private float _maxSecondValue;

    private float _middleSecondValue;
    private States _selectedState;

    private enum States
    {
        happy,
        indifferent,
        scaring
    }

    private enum Variables
    {
        sleeping
    }

    private void Start()
    {
        GenerateRandomDelay();
        StartCoroutine(ChangeEmotionAfterTime(_middleSecondValue));
    }

    private void GenerateRandomDelay()
    {
        _middleSecondValue = UnityEngine.Random.Range(_minSecondValue, _maxSecondValue);
    }

    private void GenerateRandomState()
    {
        _selectedState = GetRandomEnumValue<States>();
    }

    private IEnumerator ChangeEmotionAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        GenerateRandomState();
        _animator.SetTrigger(_selectedState.ToString());
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
        _animator.SetBool(Variables.sleeping.ToString(), true);
    }

    public void DeactivateNightTime()
    {
        _animator.SetBool(Variables.sleeping.ToString(), false);
    }
}
