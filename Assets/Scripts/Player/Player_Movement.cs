using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    
    private float _horizontalInput;
    private float _verticalInput;

    private void Update()
    {
        _verticalInput = Input.GetAxis(Directions.Vertical.ToString());
        _horizontalInput = Input.GetAxis(Directions.Horizontal.ToString());
    }

    private void FixedUpdate()
    {
        Movement();
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
}
