using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Code.Scripts.Placeholder
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class EnemyRobot : MonoBehaviour {   
        [Header("Wandering")]
        [Tooltip("List of all possible points.")]
        [SerializeField] private List<Transform> _wanderPoints;
        [Tooltip("Distance under which the robot considers point as reached.")]
        [SerializeField] private float _wanderMargin;
        [Tooltip("Default movement speed")]
        [SerializeField] private float _speed;
        private NavMeshAgent _navMeshAgent;
        private Transform _wanderTarget;
        private int _wanderPointIndex;

        [FormerlySerializedAs("_playerRaycastObject")]
        [Header("Attacking")] 
        [Tooltip("Self-explanatory.")]
        [SerializeField] private Transform _attackTarget;
        [Tooltip("Object emitting the raycast to detect the player")]
        [SerializeField] private GameObject _raycastObject;
        [Tooltip("Distance of awareness")]
        [SerializeField] private float _awarenessDistance;
        [Tooltip("Time (in s), during which player will be chased")]
        [SerializeField] private float _awarenessTime;
        [Tooltip("Maxium speed to reach while chasing.")]
        [SerializeField] private float _speedMultiplier;    
        [Tooltip("Layer Masks to target player only")] 
        [SerializeField] private LayerMask _envMask, _playerMask;
        private RaycastHit _hit;
        private bool _attackState;

        //These variables are temporary ones, only to be used as loop counters or other small tasks
        private float _awnst;

        private void Awake() {
            _attackState = false;
            _awnst = _awarenessTime;
        }
    
        private void Start() {
            _navMeshAgent = this.GetComponent<NavMeshAgent>();
            _navMeshAgent.speed = _speed;
            SelectPoint();
        }

        private void Update() {
            if (_attackState == true) {
                if (Vector3.Distance(transform.position, _attackTarget.transform.position) > _awarenessDistance) {
                    EndChase();
                } else {
                    _awnst -= Time.deltaTime;
                    _navMeshAgent.SetDestination(_attackTarget.transform.position);
                }
            }

            if (Vector3.Distance(transform.position, _wanderTarget.position) <= _wanderMargin)
                SelectPoint();
        
            if (Physics.Raycast(_raycastObject.transform.position, transform.forward, _awarenessDistance, _envMask)) {
                Debug.Log("Hit the environment, nothing here");
            } else if (Physics.Raycast(_raycastObject.transform.position, transform.forward, _awarenessDistance, _playerMask)) {
                Debug.Log("Player spotted");
                BeginChase();
            }
        }

        private void SelectPoint() {
            _wanderPointIndex = Random.Range(0, _wanderPoints.Count);
            _wanderTarget = _wanderPoints[_wanderPointIndex];
            _navMeshAgent.SetDestination(_wanderTarget.transform.position);
        }

        private void BeginChase() {
            _attackState = true;
            _awnst = _awarenessTime;
            _navMeshAgent.SetDestination(_attackTarget.transform.position);
            _navMeshAgent.speed = _speed * _speedMultiplier;
        }

        private void EndChase() {
            _navMeshAgent.speed = _speed;
            _attackState = false;
            SelectPoint();
        }
    }
}
