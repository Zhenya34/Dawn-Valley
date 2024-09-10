using UnityEngine;
using AnimControllerNamespace;

public class GlobalPetAnimController : MonoBehaviour, INightTimeController
{
    [SerializeField] private Animator _animator;

    private enum Variables
    {
        running,
        sleeping
    }

    public void SetRunningAnimation()
    {
        _animator.SetBool(Variables.sleeping.ToString(), false);
        _animator.SetBool(Variables.running.ToString(), true);
    }

    public void ActivateNightTime()
    {
        _animator.SetBool(Variables.sleeping.ToString(), true);
        _animator.SetBool(Variables.running.ToString(), false);
    }

    public void DeactivateNightTime()
    {
        _animator.SetBool(Variables.sleeping.ToString(), false);
        _animator.SetBool(Variables.running.ToString(), false);
    }
}
