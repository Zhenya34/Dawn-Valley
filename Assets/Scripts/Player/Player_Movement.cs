using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5;
    
    private Rigidbody2D _rb;
    private float _horizontalInput;
    private float _verticalInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

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
        Vector2 movement = new Vector2(_horizontalInput, _verticalInput).normalized;
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * movement);
    }
}
