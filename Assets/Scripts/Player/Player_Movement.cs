using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5;
    
        private float _horizontalInput;
        private float _verticalInput;
        private bool _isMovementEnable = true;

        private void Update()
        {
            _verticalInput = Input.GetAxis(Directions.Vertical.ToString());
            _horizontalInput = Input.GetAxis(Directions.Horizontal.ToString());
        }

        private void FixedUpdate()
        {
            if (_isMovementEnable)
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
            transform.position += moveSpeed * Time.deltaTime * movement.normalized;
        }

        public void AllowMovement()
        {
            _isMovementEnable = true;
        }

        public void ProhibitMovement()
        {
            _isMovementEnable = false;
        }
    }
}
