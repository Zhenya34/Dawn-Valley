using System.Collections;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeBeforeBlinking;
    [SerializeField] private ToolIconManager _toolManager;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _isIdle;
    private int _toolType = 0;
    private Vector2 _lastMovementDirection;
    private bool _activateBlinkingRunning = false;
    private bool _toolsAllowed = true;

    private void Update()
    {
        _verticalInput = Input.GetAxisRaw(Directions.Vertical.ToString());
        _horizontalInput = Input.GetAxisRaw(Directions.Horizontal.ToString());

        _isIdle = (_horizontalInput == 0 && _verticalInput == 0 && !Input.anyKey);

        UpdateLastMovementDirection();
        ChangeToolType();
        SetBlendValue();

        if (_isIdle)
        {
            if (!_activateBlinkingRunning && _animator.GetBool(AnimationState.IsBlinking.ToString()) == false)
            {
                _activateBlinkingRunning = true;
                StartCoroutine(ActivateBlinking());
            }
        }
        else
        {
            StopCoroutine(ActivateBlinking());
            _animator.SetBool(AnimationState.IsBlinking.ToString(), false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ActivateToolAnimTrigger();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ActivateShield();
        }
    }

    private void UpdateLastMovementDirection()
    {
        if (_horizontalInput != 0)
        {
            _lastMovementDirection = (_horizontalInput > 0) ? Vector2.right : Vector2.left;
        }
        else if (_verticalInput != 0)
        {
            _lastMovementDirection = (_verticalInput > 0) ? Vector2.up : Vector2.down;
        }
        else if(_horizontalInput == 0 && _verticalInput == 0)
        {
            _animator.SetBool(AnimationState.IsMoving.ToString(), false);
        }

        bool isMoving = _horizontalInput != 0 || _verticalInput != 0;
        _animator.SetBool(AnimationState.IsMoving.ToString(), isMoving);
    }

    private enum Directions
    {
        Vertical,
        Horizontal
    }

    private enum AnimationState
    {
        Blend,
        ToolType,
        IsActive,
        IsMoving,
        IsBlinking,
        MouseScrollWheel,
        RightButtonIsActive,
    }

    private void SetBlendValue()
    {
        float blendValue = 0f;

        if (_lastMovementDirection == Vector2.up)
        {
            blendValue = 0.66f;
        }
        else if (_lastMovementDirection == Vector2.right)
        {
            blendValue = 1f;
        }
        else if (_lastMovementDirection == Vector2.down)
        {
            blendValue = 0.33f;
        }
        else if(_lastMovementDirection == Vector2.left)
        {
            blendValue = 0f;
        }

        _animator.SetFloat(AnimationState.Blend.ToString(), blendValue);
    }

    private void ActivateToolAnimTrigger()
    {
        if(_toolsAllowed == true)
        {
            _animator.SetTrigger(AnimationState.IsActive.ToString());
        }
    }

    private void ActivateShield()
    {
        if (_toolsAllowed == true)
        {
            _animator.SetTrigger(AnimationState.RightButtonIsActive.ToString());
        }
    }

    public int GetToolTypeValue()
    {
        return _toolType;
    }

    public bool GetToolsUsingValue()
    {
        return _toolsAllowed;
    }

    private void ChangeToolType()
    {
        float scroll = Input.GetAxis(AnimationState.MouseScrollWheel.ToString());
        if (scroll != 0)
        {
            _toolType += (scroll > 0 ? 1 : -1);

            if (_toolType > 6)
            {
                _toolType = 0;
            }
            else if (_toolType < 0)
            {
                _toolType = 6;
            }

            _animator.SetInteger(AnimationState.ToolType.ToString(), _toolType);
        }
        _toolManager.UpdateToolIcon(_toolType);
    }

    private IEnumerator ActivateBlinking()
    {
        yield return new WaitForSeconds(_timeBeforeBlinking);
        _animator.SetBool(AnimationState.IsBlinking.ToString(), true);
        _activateBlinkingRunning = false;
    }

    public void AllowToolsUsing()
    {
        _toolsAllowed = true;
    }

    public void ProhibitToolsUsing()
    {
        _toolsAllowed = false;
    }
}