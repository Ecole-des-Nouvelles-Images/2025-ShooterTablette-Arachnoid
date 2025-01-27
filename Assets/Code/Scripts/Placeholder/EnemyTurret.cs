using System;
using UnityEngine;

public class EnemyTurret : MonoBehaviour {
    [Header("Tracking")]
    [Tooltip("Turret rotating object")]
    [SerializeField] private GameObject Turret;

    [SerializeField] private ParticleSystem _cannonParticles1, _cannonParticles2; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("AAAAA");
            _cannonParticles1.Play();
        }
    }
}