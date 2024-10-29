using System.Collections;
using UnityEngine;
using static ToolSwitcher;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _timeBeforeBlinking;
    [SerializeField] private ToolSwitcher _toolSwitcher;
    [SerializeField] private float _holdThresholdForLeftButton;
    [SerializeField] private float _holdThresholdForRightButton;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _isIdle;
    private Vector2 _lastMovementDirection;
    private bool _activateBlinkingRunning = false;
    private bool _toolsAllowed = true;
    private float _holdTimer = 0.0f;
    private bool _isLeftButtonHolding = false;
    private bool _isRightButtonHolding = false;

    private void Update()
    {
        _verticalInput = Input.GetAxisRaw(Directions.Vertical.ToString());
        _horizontalInput = Input.GetAxisRaw(Directions.Horizontal.ToString());

        _isIdle = (_horizontalInput == 0 && _verticalInput == 0 && !Input.anyKey);

        UpdateLastMovementDirection();
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
            ActivateLeftToolTrigger();
        }

        if (Input.GetMouseButtonDown(1))
        {
            ActivateRightToolTrigger();
        }

        HandleButtonHold(0, ref _isLeftButtonHolding, _holdThresholdForLeftButton, AnimationState.LeftButtonIsHolding.ToString());
        HandleButtonHold(1, ref _isRightButtonHolding, _holdThresholdForRightButton, AnimationState.RightButtonIsHolding.ToString());
    }

    private void HandleButtonHold(int mouseButton, ref bool isButtonHolding, float holdThreshold, string animationState)
    {
        if (Input.GetMouseButton(mouseButton))
        {
            _holdTimer += Time.deltaTime;

            if (_holdTimer >= holdThreshold && !isButtonHolding)
            {
                isButtonHolding = true;
                _animator.SetBool(animationState, true);
            }
        }
        else
        {
            if (isButtonHolding)
            {
                isButtonHolding = false;
                _holdTimer = 0.0f;
                _animator.SetBool(animationState, false);
            }
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
        IsMoving,
        IsBlinking,
        LeftButtonIsActive,
        RightButtonIsActive,
        LeftButtonIsHolding,
        RightButtonIsHolding
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

    private void ActivateLeftToolTrigger()
    {
        if (_toolsAllowed == true)
        {
            _animator.SetTrigger(AnimationState.LeftButtonIsActive.ToString());
        }
    }

    private void ActivateRightToolTrigger()
    {
        if (_toolsAllowed == true)
        {
            _animator.SetTrigger(AnimationState.RightButtonIsActive.ToString());
        }
    }

    public void UpdateToolType(ToolType _toolType)
    {
        int toolIndex = (int)_toolType;
        _animator.SetInteger(AnimationState.ToolType.ToString(), toolIndex);
    } 

    public bool GetToolsUsingValue()
    {
        return _toolsAllowed;
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