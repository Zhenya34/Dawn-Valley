using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Dictionary<Vector2, Sprite> _directionSprites;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _blinkCoroutineIsRunning = false;
    private bool _isIdle;
    private bool _wasIdle = false;
    private int _toolType = 0;
    private Vector2 _lastMovementDirection;

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private float _blinkTimer;
    [SerializeField] private Sprite[] _spritesDirections = new Sprite[4];
    [SerializeField] private Sprite _blinkSpriteRight;
    [SerializeField] private Sprite _blinkSpriteLeft;
    [SerializeField] private Sprite _blinkSpriteDown;
    [SerializeField] private Animator _animator;


    private void Start()
    {
        _directionSprites = new Dictionary<Vector2, Sprite>
        {
            { Vector2.down, _spritesDirections[0] },
            { Vector2.up, _spritesDirections[1] },
            { Vector2.right, _spritesDirections[2] },
            { Vector2.left, _spritesDirections[3] }
        };
    }

    private void Update()
    {
        _verticalInput = Input.GetAxisRaw(Directions.Vertical.ToString());
        _horizontalInput = Input.GetAxisRaw(Directions.Horizontal.ToString());

        _isIdle = (_horizontalInput == 0 && _verticalInput == 0 && !Input.anyKey);

        UpdatePlayerSprites();

        if (_isIdle)
        {
            if (!_blinkCoroutineIsRunning)
            {
                StartCoroutine(BlinkAnimation());
            }
        }
        else
        {
            StopCoroutine(BlinkAnimation());
        }

        if (Input.GetMouseButtonDown(0))
        {
            ActivateToolAnimTrigger();
        }

        ChangeToolType();
        SetBlendValue();
    }

    private void UpdatePlayerSprites()
    {
        if (_horizontalInput != 0)
        {
            _sr.sprite = (_horizontalInput > 0) ? _spritesDirections[2] : _spritesDirections[3];
            _lastMovementDirection = (_horizontalInput > 0) ? Vector2.right : Vector2.left;
        }
        else if (_verticalInput != 0)
        {
            _sr.sprite = (_verticalInput > 0) ? _spritesDirections[1] : _spritesDirections[0];
            _lastMovementDirection = (_verticalInput > 0) ? Vector2.up : Vector2.down;
        }
        else if(_horizontalInput == 0 && _verticalInput == 0)
        {
            _animator.SetBool(AnimationState.IsMoving.ToString(), false);
        }

        bool isMoving = _horizontalInput != 0 || _verticalInput != 0;
        _animator.SetBool(AnimationState.IsMoving.ToString(), isMoving);
    }

    private void SetSpriteFromLastDirection()
    {
        if(_wasIdle == false)
        {
            if (_lastMovementDirection == Vector2.up)
            {
                _sr.sprite = _spritesDirections[1];
            }
            else if (_lastMovementDirection == Vector2.right)
            {
                _sr.sprite = _spritesDirections[2];
            }
            else if (_lastMovementDirection == Vector2.down)
            {
                _sr.sprite = _spritesDirections[0];
            }
            else if (_lastMovementDirection == Vector2.left)
            {
                _sr.sprite = _spritesDirections[3];
            }
            _wasIdle = true;
        }
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
        MouseScrollWheel
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
        _animator.SetTrigger(AnimationState.IsActive.ToString());
    }

    private void ChangeToolType()
    {
        float scroll = Input.GetAxis(AnimationState.MouseScrollWheel.ToString());
        if (scroll != 0)
        {
            _toolType += (scroll > 0 ? 1 : -1);

            if (_toolType > 5)
            {
                _toolType = 0;
            }
            else if (_toolType < 0)
            {
                _toolType = 5;
            }

            _animator.SetInteger(AnimationState.ToolType.ToString(), _toolType);
        }
    }

    private IEnumerator BlinkAnimation()
    {
        _blinkCoroutineIsRunning = true;
        yield return new WaitForSeconds(_blinkTimer);
        
        if(_lastMovementDirection == Vector2.down)
        {
            _sr.sprite = _blinkSpriteDown;
        }
        else if (_lastMovementDirection == Vector2.right)
        {
            _sr.sprite = _blinkSpriteRight;
        }
        else if (_lastMovementDirection == Vector2.left)
        {  
            _sr.sprite = _blinkSpriteLeft;
        }

        yield return new WaitForSeconds(0.5f);
        _wasIdle = false;
        SetSpriteFromLastDirection();
        _blinkCoroutineIsRunning = false;
    }
}