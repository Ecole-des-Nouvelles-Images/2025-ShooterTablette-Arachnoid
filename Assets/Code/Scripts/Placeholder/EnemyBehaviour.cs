using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private List<Transform> _wanderPoints;
    [SerializeField] private float _wanderMargin;
    
    private NavMeshAgent _navMeshAgent;
    private Transform _wanderTarget;
    private int _wanderPointIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        SelectPoint();
    }

    // Update is called once per frame
    void Update() {
        if (Vector3.Distance(transform.position, _wanderTarget.position) <= _wanderMargin) {
            SelectPoint();
        }
    }

    void SelectPoint() {
        Debug.Log("Selecting new point");
        _wanderPointIndex = Random.Range(0, _wanderPoints.Count);
        _wanderTarget = _wanderPoints[_wanderPointIndex];
        _navMeshAgent.SetDestination(_wanderTarget.transform.position);
    }
}
