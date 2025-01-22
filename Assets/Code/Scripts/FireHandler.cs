using UnityEngine;

namespace Code.Scripts
{
    public class FireHandler : MonoBehaviour
    {
        [SerializeField] private Transform _canonTip;
        private ParticleSystem _bulletParticleSystem;
        private PlayerController _playerController;

        private bool _wasFiring;

        private void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _bulletParticleSystem = _canonTip.GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (_playerController.IsFiring && !_wasFiring)
            {
                _bulletParticleSystem.Play();
                _wasFiring = true;
            }
            else if (!_playerController.IsFiring && _wasFiring)
            {
                _bulletParticleSystem.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                _wasFiring = false;
            }
        }
    }
}