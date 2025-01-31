using System.Collections;
using UnityEngine;

namespace Code.Scripts.LoadingSystem
{
    public class SceneBuilder : MonoBehaviour
    {
        [SerializeField] private Vector3 _worldOffset;

        public IEnumerator ActivateScene(float simulatedTime)
        {
            GameObject sceneRoot = GameObject.Find("Root");

            sceneRoot.transform.position += _worldOffset;

            yield return new WaitForSeconds(simulatedTime);

            sceneRoot.SetActive(true);
        }

        public IEnumerator Build()
        {
            yield return BulkRoutines.Instance.BulkInstantiateCoroutine();

            yield return BulkRoutines.Instance.BlockingLoad(20);
        }
    }
}
