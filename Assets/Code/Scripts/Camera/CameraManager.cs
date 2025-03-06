using Unity.Cinemachine;
using UnityEngine;

namespace Code.Scripts.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _brainCam;
        [SerializeField] private CinemachineCamera _cinemachineCam;

        private void OnEnable()
        {
            GameEvents.OnPlayerSpawn += SetCamera;
        }

        private void OnDestroy()
        {
            GameEvents.OnPlayerSpawn -= SetCamera;
        }

        private void SetCamera(GameObject player)
        {
            _cinemachineCam.Follow = player.transform;
            // _cinemachineCam.AddComponent<CinemachineFollow>();
            // _cinemachineCam.transform.GetComponent<CinemachineFollow>().FollowOffset
        }
    }
}
