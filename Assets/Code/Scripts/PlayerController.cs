using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Code.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _fireSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        private float _moveSpeed;
        private PlayerInput _playerInput;
        private Rigidbody _rb;
        public bool IsFiring = false;

        private InputAction _moveAction;
        private InputAction _lookAction;

        public Button FireButton;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Move"];
            _lookAction = _playerInput.actions["Look"];

            var fireButtonEventTrigger = FireButton.GetComponent<EventTrigger>();
            if (fireButtonEventTrigger == null)
            {
                fireButtonEventTrigger = FireButton.gameObject.AddComponent<EventTrigger>();
            }

            AddEventTrigger(fireButtonEventTrigger, EventTriggerType.PointerDown, (eventData) => { IsFiring = true; });
            AddEventTrigger(fireButtonEventTrigger, EventTriggerType.PointerUp, (eventData) => { IsFiring = false; });
        }

        private void FixedUpdate()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

            if (moveDirection != Vector3.zero)
            {
                _rb.MovePosition(_rb.position + moveDirection * (_moveSpeed * Time.fixedDeltaTime));
            }

            Vector2 lookInput = _lookAction.ReadValue<Vector2>();
            if (lookInput != Vector2.zero)
            {
                Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            _moveSpeed = IsFiring ? _fireSpeed : _sprintSpeed;
        }

        private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener((eventData) => callback(eventData));
            trigger.triggers.Add(entry);
        }
    }
}
