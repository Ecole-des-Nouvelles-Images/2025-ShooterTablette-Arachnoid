using UnityEngine;

namespace Code.Scripts.Placeholder
{
    public class EnemyRobotSwarm : MonoBehaviour {
        [Header("Spawning")]
        [Tooltip("Enemy prefab"), SerializeField] private GameObject _enemyPrefab;
        [Tooltip("Target of all members of the swarm"), SerializeField] private GameObject _target;
        [Tooltip("Minimum & Maximum distance from player to spawn"), SerializeField] private float _minDistance, _maxDistance;
        [Tooltip("Interval between enemy spawns"), SerializeField] private float _spawnInterval;

        private GameObject _instance;
        private float _sint;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update() {
            if (_sint <= 0) {
                SpawnEnemy();
                _sint = _spawnInterval;
            }
            _sint -= Time.deltaTime;
        }

        private void SpawnEnemy() {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            _instance = Instantiate(_enemyPrefab, this.transform.position, Quaternion.identity);
            _instance.GetComponent<EnemyRobot_Mindless>()._target = _target;
        }
    }
}
