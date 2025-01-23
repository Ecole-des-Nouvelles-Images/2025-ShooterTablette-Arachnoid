using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyRobot_Mindless : MonoBehaviour {
    
    [Header("Detection")] 
    [Tooltip("Target object")] public GameObject _target;
    [Tooltip("Raycast origin"), SerializeField] private GameObject _raycastObject;
    [Tooltip("Player Layer Mask"), SerializeField] private LayerMask _playerMask;
    [Tooltip("Maximum detection distance"), SerializeField] private float _rayDistance;
    
    [Header("Movement")]
    [Tooltip("Self-explanatory")] private NavMeshAgent _navMeshAgent;
    [Tooltip("Default speed"), SerializeField] private float _baseSpeed;
    [Tooltip("Speed multiplier"), SerializeField] private float _speedFactor;
    [Tooltip("Speeding time after player sight"), SerializeField] private float _speedTime;


    private Rigidbody _rb;
    private RaycastHit _hit;
    private float _spdt;
    private bool _speeding;
    
    void Start() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _navMeshAgent.speed = _baseSpeed;
    }

    void Update() {

        if (_speeding && _spdt > 0) {
            _spdt -= Time.deltaTime;
        } else {
            _navMeshAgent.speed = _baseSpeed;
            _navMeshAgent.SetDestination(_target.transform.position);
        }
        
        if (Physics.Raycast(_raycastObject.transform.position, transform.forward, out _hit, _rayDistance, _playerMask)) {
            _speeding = true;
            _spdt = _speedTime;
            _navMeshAgent.speed = _baseSpeed * _speedFactor;
            _navMeshAgent.SetDestination(_hit.point);
        }
    }

    private void OnGUI() {
        Debug.DrawRay(_raycastObject.transform.position, transform.forward * _rayDistance);
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
