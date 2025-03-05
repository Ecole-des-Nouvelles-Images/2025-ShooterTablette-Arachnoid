using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;
using TMPro;

using Code.Scripts.Camera;
using Code.Scripts.Player;
using DG.Tweening;

namespace Code.Scripts.UI.Toolbox
{
    public class InGameControlDrawer : MonoBehaviour
    {
        [Header("Panel position")]
        [SerializeField] private RectTransform _root;
        [Tooltip("Size of the root panel represented by the difference between verticals anchors of the panel (Top - Bottom)")]
        [SerializeField] private Button _handle;
        [SerializeField] private float _drawerAnimationDuration;

        [Header("UI Elements: Camera")]
        [SerializeField] private Button _toggleButtonPerspective;
        [SerializeField] private Button _toggleButtonOrthographic;
        [SerializeField] private Slider _cameraLensSlider;
        [SerializeField] private TMP_Text _cameraLensValue;
        [SerializeField] private Slider _cameraPerspectiveDistanceSlider;
        [SerializeField] private TMP_Text _cameraPerspectiveDistanceValue;

        [Header("UI Elements: Player")]
        [SerializeField] private Slider _speedSlider;
        [SerializeField] private TMP_Text _speedValue;
        [SerializeField] private Slider _sizeSlider;
        [SerializeField] private TMP_Text _sizeValue;

        private CameraMode _currentCamera = CameraMode.Perspective;
        private bool _toolbarActive = false;
        private float _verticalAnchorDelta;

        private (GameObject Object, CinemachineCamera Camera, CinemachinePositionComposer Composer) _perspectiveView;
        private (GameObject Object, CinemachineCamera Camera) _orthographicView;
        private PlayerController _player;

        private readonly Vector2 _cameraPerspectiveLensRange = new (1, 100);
        private readonly Vector2 _cameraPerspectiveDistanceRange = new (1, 70);
        private readonly Vector2 _cameraOrthographicLensRange = new (1, 30);
        private readonly Vector2 _playerSpeedRange = new (5, 30);
        private readonly Vector2 _playerSizeRange = new (0.1f, 5);

        private Color _disabledColor = new(1, 1, 1, .1f);

        private void Awake()
        {
            GameObject perspectiveCam = GameObject.Find("Camera/Perspective");
            GameObject orthographicCam = GameObject.Find("Camera/Orthographic");

            _perspectiveView = new (perspectiveCam, perspectiveCam.GetComponentInChildren<CinemachineCamera>(), perspectiveCam.GetComponentInChildren<CinemachinePositionComposer>());
            _orthographicView = new (orthographicCam, orthographicCam.GetComponentInChildren<CinemachineCamera>());
            _player = FindFirstObjectByType<PlayerController>();
        }

        private void Start()
        {
            _verticalAnchorDelta = _root.anchorMax.y - _root.anchorMin.y;

            SwitchCamera(CameraMode.Perspective);

            SetSlider(_speedSlider, _playerSpeedRange, _speedValue, _player.MoveSpeed);
            SetSlider(_sizeSlider, _playerSizeRange, _sizeValue ,_player.transform.localScale.x);
        }

        private void OnEnable()
        {
            _handle.onClick.AddListener(ToggleToolbar);
            _toggleButtonPerspective.onClick.AddListener(delegate { OnToggleChange(CameraMode.Perspective); });   // TODO: Recursive call when setting the value
            _toggleButtonOrthographic.onClick.AddListener(delegate { OnToggleChange(CameraMode.Orthographic); }); // TODO: Recursive call when setting the value
            _cameraLensSlider.onValueChanged.AddListener(delegate { OnCameraLensChange(); });
            _cameraPerspectiveDistanceSlider.onValueChanged.AddListener(delegate { OnCameraDistanceChange(); });
            _speedSlider.onValueChanged.AddListener(delegate { OnPlayerSpeedChange(); });
            _sizeSlider.onValueChanged.AddListener(delegate { OnPlayerSizeChange(); });
        }

        private void OnDisable()
        {
            _handle.onClick.RemoveAllListeners();
            _toggleButtonPerspective.onClick.RemoveAllListeners();
            _toggleButtonOrthographic.onClick.RemoveAllListeners();
            _cameraLensSlider.onValueChanged.RemoveAllListeners();
            _cameraPerspectiveDistanceSlider.onValueChanged.RemoveAllListeners();
            _speedSlider.onValueChanged.RemoveAllListeners();
            _sizeSlider.onValueChanged.RemoveAllListeners();
        }

        public void ToggleToolbar()
        {
            _toolbarActive = !_toolbarActive;

            _root.DOKill();
            _handle.DOKill();

            if (_toolbarActive)
            {
                _root.DOAnchorMin(new Vector2(_root.anchorMin.x, _root.anchorMin.y - _verticalAnchorDelta), _drawerAnimationDuration).SetEase(Ease.OutExpo);
                _root.DOAnchorMax(new Vector2(_root.anchorMax.x, _root.anchorMax.y - _verticalAnchorDelta), _drawerAnimationDuration).SetEase(Ease.OutExpo);
                _handle.GetComponent<Image>().DOColor(new Color(1, 1, 1, .78f), _drawerAnimationDuration).SetEase(Ease.InOutExpo).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _root.DOAnchorMin(new Vector2(_root.anchorMin.x, _root.anchorMin.y + _verticalAnchorDelta), _drawerAnimationDuration).SetEase(Ease.OutExpo);
                _root.DOAnchorMax(new Vector2(_root.anchorMax.x, _root.anchorMax.y + _verticalAnchorDelta), _drawerAnimationDuration).SetEase(Ease.OutExpo);
                _handle.GetComponent<Image>().DOColor(new Color(0, 0, 0, 0.78f), _drawerAnimationDuration).SetEase(Ease.InOutExpo).SetLoops(-1, LoopType.Yoyo);
            }
        }

        private void SwitchCamera(CameraMode mode)
        {
            bool isPerspective = mode == CameraMode.Perspective;
            float lensSize = isPerspective ? _perspectiveView.Camera.Lens.FieldOfView : _orthographicView.Camera.Lens.OrthographicSize;
            Color sliderColor = isPerspective ? Color.white : _disabledColor;

            _currentCamera = mode;
            _perspectiveView.Object.SetActive(isPerspective);
            _orthographicView.Object.SetActive(!isPerspective);

            _toggleButtonOrthographic.transform.Find("Background/Icon").gameObject.SetActive(!isPerspective);
            _toggleButtonOrthographic.interactable = isPerspective;
            _toggleButtonPerspective.transform.Find("Background/Icon").gameObject.SetActive(isPerspective);
            _toggleButtonPerspective.interactable = !isPerspective;

            _cameraPerspectiveDistanceSlider.interactable = isPerspective;
            _cameraPerspectiveDistanceSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = sliderColor;
            _cameraPerspectiveDistanceSlider.transform.parent.Find("Value").GetComponent<TMP_Text>().color = sliderColor;
            _cameraPerspectiveDistanceSlider.transform.parent.parent.Find("Label").GetComponent<TMP_Text>().color = sliderColor;

            SetSlider(_cameraLensSlider, (isPerspective ? _cameraPerspectiveLensRange : _cameraOrthographicLensRange), _cameraLensValue, lensSize);

            if (isPerspective)
                SetSlider(_cameraPerspectiveDistanceSlider, _cameraPerspectiveDistanceRange, _cameraPerspectiveDistanceValue, _perspectiveView.Composer.CameraDistance);
            else
                _orthographicView.Camera.Lens.NearClipPlane = -50;
        }

        #region Callbacks

        public void OnToggleChange(CameraMode mode)
        {
            SwitchCamera(mode);
        }

        public void OnCameraLensChange()
        {
            float lensSize = _cameraLensSlider.value;

            if (_currentCamera == CameraMode.Perspective)
                _perspectiveView.Camera.Lens.FieldOfView = lensSize;
            else
                _orthographicView.Camera.Lens.OrthographicSize = lensSize;

            _cameraLensValue.text = lensSize.ToString("F2");
        }

        public void OnCameraDistanceChange()
        {
            float distance = _cameraPerspectiveDistanceSlider.value;

            _perspectiveView.Composer.CameraDistance = distance;
            _cameraPerspectiveDistanceValue.text = distance.ToString("F2");
        }

        public void OnPlayerSpeedChange()
        {
            float speed = _speedSlider.value;

            _player.MoveSpeed = speed;
            _speedValue.text = speed.ToString("F2");
        }

        private void OnPlayerSizeChange()
        {
            float factor = _sizeSlider.value;
            Vector3 scale = new Vector3(factor, factor, factor);

            _player.transform.localScale = scale;
            _sizeValue.text = factor.ToString("F2");
        }

        #endregion

        #region Utils

        private void SetSlider(Slider slider, Vector2 range, TMP_Text box, float value = -1f)
        {
            slider.minValue = range.x;
            slider.maxValue = range.y;

            if (value >= slider.minValue && value <= slider.maxValue)
            {
                slider.value = value;
                box.text = value.ToString("F2");
            }
        }

        #endregion
    }
}
