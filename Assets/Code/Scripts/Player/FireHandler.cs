using UnityEngine;

namespace Code.Scripts.Player
{
    public class FireHandler : MonoBehaviour
    {
        [SerializeField] private Transform _canonTip;

        private ParticleSystem[] _bulletParticleSystems;

        private PlayerController _playerController;

        private bool _wasFiring;

        private void Start()
        {
            _playerController = GetComponentInParent<PlayerController>();
            _bulletParticleSystems = _canonTip.GetComponentsInChildren<ParticleSystem>();
        }

        private void Update()
        {
            if (_playerController.IsFiring && !_wasFiring)
            {
                foreach(ParticleSystem ps in _bulletParticleSystems)
                    ps.Play();

                _wasFiring = true;
            }
            else if (!_playerController.IsFiring && _wasFiring)
            {
                foreach (ParticleSystem ps in _bulletParticleSystems)
                    ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);

                _wasFiring = false;
            }
        }
    }
}
