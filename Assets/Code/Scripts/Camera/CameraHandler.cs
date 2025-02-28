using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Transform _player;
        [SerializeField]private CinemachineCamera _cinemachineCamera;
        private CinemachinePositionComposer _positionComposer;

        private void Start()
        {
            _playerInput = GetComponentInParent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _player = GetComponentInParent<Transform>();
            _positionComposer = _cinemachineCamera.GetComponent<CinemachinePositionComposer>();

        }

        private void FixedUpdate()
        {
            Vector2 moveInput = _moveAction.ReadValue<Vector2>();
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);
            _positionComposer.TargetOffset.z = Mathf.Lerp(0, 8, Mathf.Abs(moveDirection.z));
        }
    }
}