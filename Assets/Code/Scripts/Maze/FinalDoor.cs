using UnityEngine;

namespace Code.Scripts.Maze
{
    public class FinalDoor : MonoBehaviour
    {
        [SerializeField] private GameObject _doorSkin;
        [SerializeField] private MeshRenderer _fifthDisplay;
        [SerializeField] private MeshRenderer[] _firstDisplays;
        [SerializeField] private MeshRenderer[] _secondDisplays;
        [SerializeField] private MeshRenderer[] _thirdDisplays;
        [SerializeField] private MeshRenderer[] _fourthDisplays;
        [SerializeField] private Material _activeMaterial;
        [SerializeField] private Material _inactiveMaterial;

        private float _activeHivesQtt;
        private float _destroyedHivesQtt;
        private float _cooldownUpdate;
        private BoxCollider _myCollider;

        private void Awake()
        {
            _myCollider = transform.GetComponent<BoxCollider>();
        }

        private void Start()
        {
            SwitchToInactiveMaterial(_firstDisplays);
            SwitchToInactiveMaterial(_secondDisplays);
            SwitchToInactiveMaterial(_thirdDisplays);
            SwitchToInactiveMaterial(_fourthDisplays);
            _fifthDisplay.material = _inactiveMaterial;
            _myCollider.enabled = true;
            _doorSkin.SetActive(true);
        }
        private void Update()
        {
            if(_cooldownUpdate >= 1)
            {
                UpdateDisplays();
                _cooldownUpdate = 0;
            }
            else
            {
                _cooldownUpdate += Time.deltaTime;
            }
        }
        private void UpdateDisplays()
        {
            if (_activeHivesQtt > 0 && _destroyedHivesQtt > 0)
            {
                if (_destroyedHivesQtt/_activeHivesQtt > 0.25f)
                    SwitchToActiveMaterial(_firstDisplays);
                if (_destroyedHivesQtt/_activeHivesQtt > 0.5f)
                    SwitchToActiveMaterial(_secondDisplays);
                if (_destroyedHivesQtt/_activeHivesQtt > 0.75f)
                    SwitchToActiveMaterial(_thirdDisplays);
                if (_destroyedHivesQtt/_activeHivesQtt >= 1)
                {
                    SwitchToActiveMaterial(_fourthDisplays);
                    OpenTheDoor();
                }
            }
            else if (_activeHivesQtt == 0)
            {
                SwitchToActiveMaterial(_firstDisplays);
                SwitchToActiveMaterial(_secondDisplays);
                SwitchToActiveMaterial(_thirdDisplays);
                SwitchToActiveMaterial(_fourthDisplays);
                OpenTheDoor();
            }
        }
        private void SwitchToActiveMaterial(MeshRenderer[] displays)
        {
            foreach (MeshRenderer display in displays)
            {
                display.material = _activeMaterial;
            }
        }
        private void SwitchToInactiveMaterial(MeshRenderer[] displays)
        {
            foreach (MeshRenderer display in displays)
            {
                display.material = _inactiveMaterial;
            }
        }
        private void OpenTheDoor()
        {
            _myCollider.enabled = false;
            _doorSkin.SetActive(false);
            _fifthDisplay.material = _activeMaterial;
        }
    }
}
