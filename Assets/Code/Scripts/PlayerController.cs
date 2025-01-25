using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        //[SerializeField] private float _energy = 100f;
        
        [SerializeField] private float _moveSpeed = 5f;
        private PlayerInput _playerInput;
        private Rigidbody _rb;
        public bool IsFiring = false;


        void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();
        }

        void FixedUpdate()
        {
            Vector2 moveInput = _playerInput.actions["Move"].ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            if (moveDirection != Vector3.zero)
            {
                _rb.MovePosition(_rb.position + moveDirection * (_moveSpeed * Time.fixedDeltaTime));
            }

            Vector2 lookInput = _playerInput.actions["Look"].ReadValue<Vector2>();

            if (lookInput != Vector2.zero)
            {
                Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
                transform.rotation = Quaternion.LookRotation(lookDirection);
                IsFiring = true;
            }
            else
            {
                IsFiring = false;
            }
        }
    }
}