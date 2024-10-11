using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    
    private float _horizontalInput;
    private float _verticalInput;
    private bool _isMovementAnable = true;

    private void Update()
    {
        _verticalInput = Input.GetAxis(Directions.Vertical.ToString());
        _horizontalInput = Input.GetAxis(Directions.Horizontal.ToString());
    }

    private void FixedUpdate()
    {
        if (_isMovementAnable)
        {
            Movement();
        }
    }

    private enum Directions
    {
        Vertical,
        Horizontal
    }

    private void Movement()
    {
        Vector3 movement = new(_horizontalInput, _verticalInput, 0);
        transform.position += _moveSpeed * Time.deltaTime * movement.normalized;
    }

    public void AllowMovement()
    {
        _isMovementAnable = true;
    }

    public void ProhibitMovement()
    {
        _isMovementAnable = false;
    }
}
