using System.Collections;
using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.LoadingSystem
{
    public class BulkRoutines : SingletonMonoBehaviour<BulkRoutines>
    {
        [SerializeField] private Vector2 _zone = new(10, 10);
        [SerializeField] private float _quantity = 100;
        [SerializeField] private GameObject _prefab;

        public IEnumerator BulkInstantiateCoroutine()
        {
            for (int i = 0; i < _quantity; i++)
            {
                Instantiate(_prefab, new Vector3(Random.Range(0, _zone.x), Random.Range(5, 15), Random.Range(0, _zone.y)), Quaternion.identity);
                yield return null;
            }
        }

        public IEnumerator BlockingLoad(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }
    }
}
