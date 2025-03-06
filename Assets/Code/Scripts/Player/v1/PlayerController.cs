using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Code.Scripts.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public float MoveSpeed
        {
            get => _sprintSpeed;
            set => _sprintSpeed = value;
        }

        [SerializeField] private float _fireSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 8f;
        [SerializeField] private float _moveSpeed;

        private PlayerInput _playerInput;
        private Rigidbody _rb;

        public bool IsFiring = false;

        private InputAction _moveAction;
        private InputAction _lookAttackAction;

        private Button _fireButton;

        private void Start()
        {
            _fireButton = FindAnyObjectByType<Button>(); // a modifier car l'ui aura d'autres boutons
            _rb = GetComponent<Rigidbody>();
            _playerInput = GetComponent<PlayerInput>();

            _moveAction = _playerInput.actions["Move"];
            _lookAttackAction = _playerInput.actions["LookAttack"];

            var fireButtonEventTrigger = _fireButton.GetComponent<EventTrigger>();

            if (fireButtonEventTrigger == null)
            {
                fireButtonEventTrigger = _fireButton.gameObject.AddComponent<EventTrigger>();
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

            Vector2 lookInput = _lookAttackAction.ReadValue<Vector2>();

            float deadzone = 0.4f; // 10% de deadzone
            if (lookInput.magnitude > deadzone)
            {
                Vector3 lookDirection = new Vector3(lookInput.x, 0, lookInput.y);
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
            else if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }

            _moveSpeed = IsFiring ? _fireSpeed : _sprintSpeed;
        }

        private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> callback)
        {
            // var entry = new EventTrigger.Entry { eventID = eventType };
            // entry.callback.AddListener((eventData) => callback(eventData));
            // trigger.triggers.Add(entry);
        }
    }
}
