using UnityEngine;

namespace Code.Scripts.Placeholder
{
    public class EnemyTurret : MonoBehaviour {
        [Header("Tracking")]
        [Tooltip("Turret rotating object")]
        [SerializeField] private GameObject Turret;
        [SerializeField] private GameObject _cannon1, _cannon2;

        private Vector3 _lookDir;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                _lookDir = (other.transform.position - transform.position).normalized;

                Turret.transform.rotation = Quaternion.LookRotation(_lookDir, Vector3.up);
            
                Debug.DrawLine(_cannon1.transform.position, other.gameObject.transform.position);
                Debug.DrawLine(_cannon2.transform.position, other.gameObject.transform.position);
            }
        }
    
    }
}