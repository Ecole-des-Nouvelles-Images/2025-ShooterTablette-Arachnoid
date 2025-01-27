using Code.Scripts;
using Unity.Mathematics;
using UnityEngine;

public class Entry : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerParent;
    
    void Start()
    {
        GameObject player =Instantiate(_playerPrefab, transform.position, quaternion.identity, _playerParent);
        GameEvents.OnPlayerSpawn?.Invoke(player);
    }
}
