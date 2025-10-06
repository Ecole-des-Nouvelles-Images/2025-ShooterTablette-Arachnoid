using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController: MonoBehaviour
    {
        [Header("Motion")]
        [field: SerializeField] public float MoveSpeed { get; set; } = 15f;
        [field: SerializeField] [Range(0, 1)] public float MoveSpeedWhileFiringFactor { get; private set; } = .5f;

        [Header("Look-Attack")]
        [field: SerializeField] public float Torque { get; private set; } = 40f;
        [field: SerializeField] [Range(0, 1)] public float Deadzone { get; private set; } = .4f;
        [SerializeField] private GameObject _canon;                         //TODO: Replace prototype by a dedicated weapon system
        [Tooltip("Controls emission rate of attached Particle Systems")]    //TODO: Replace prototype by a dedicated weapon system
        [SerializeField] private float _fireRate = 10;                      //TODO: Replace prototype by a dedicated weapon system

        private Rigidbody _rb;
        private ParticleSystem[] _bulletParticleSystems;                    //TODO: Replace prototype by a dedicated weapon system

        public float FireRate {                   //TODO: Replace prototype by a dedicated weapon system
            get => _fireRate;
            private set
            {
                _fireRate = value;
                foreach (ParticleSystem ps in _bulletParticleSystems)
                {
                    var emission = ps.emission;
                    emission.rateOverTime = _fireRate;
                }
            }
        }                                         //TODO: Replace prototype by a dedicated weapon system
        private bool _isFiring = false;
        private Vector3 _motion;
        private Vector3 _rotation;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _bulletParticleSystems = _canon.GetComponentsInChildren<ParticleSystem>();
            FireRate = _fireRate;
        }

        private void Update()
        {
            if (_motion != Vector3.zero)
            {
                _rb.AddForce(_motion, ForceMode.Force);
                transform.rotation = Quaternion.LookRotation(_motion);
            }
        }

        public void OnMove(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            _motion = new Vector3(input.x, 0, input.y) * 100 * MoveSpeed * Time.deltaTime * (_isFiring ? MoveSpeedWhileFiringFactor : 1);
        }

        public void OnLookAttack(InputAction.CallbackContext ctx)
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            _rotation = new Vector3(input.x, 0, input.y) * Torque * Time.deltaTime;

            if (input.magnitude > Deadzone)
                transform.rotation = Quaternion.LookRotation(_rotation);

            Fire(ctx);
        }

        private void Fire(InputAction.CallbackContext ctx)
        {
            foreach (ParticleSystem ps in _bulletParticleSystems)
            {
                if (ctx.started)
                {
                    ps.Play();
                    _isFiring = true;
                }
                else if (ctx.canceled)
                {
                    ps.Stop(false, ParticleSystemStopBehavior.StopEmitting);
                    _isFiring = false;
                }
            }
        }
    }
}
