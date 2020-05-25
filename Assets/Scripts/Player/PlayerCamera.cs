using UnityEngine;
using SAMODEUS.Cameras;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private DataCamera _dataCamera = null;
    [SerializeField] private Transform _body = null;
    [SerializeField] private float _power = 0.7f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _slowDownAmount = 1f;
    [SerializeField] private int _smoothTime = 5;
    [SerializeField] private Camera _camera = null;
    [SerializeField] private HeadBobbing _headBobbing = null;
    [SerializeField] private Transform _handTransform = null;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private float _currentY = 0.0f;
    private float _currentX = 0.0f;
    private bool _shouldShake = false;
    private Vector3 _startPosition = Vector3.zero;
    private float _initialDuration = 0.0f;

    public bool ShouldShake { get { return _shouldShake; } set { _shouldShake = value; } }
    public HeadBobbing HeadBobbing { get { return _headBobbing; } }
    public Camera Camera { get { return _camera; } }
    public Transform HandTransform { get { return _handTransform; } }

    private void Start()
    {
        _startPosition = _camera.transform.localPosition;
        _initialDuration = _duration;

        GameLoopManager.Instance.Camera += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void IsPaused (bool pause)
    {
        if(pause == true)
        {
            GameLoopManager.Instance.Camera -= OnUpdate;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            GameLoopManager.Instance.Camera += OnUpdate;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnUpdate()
    {
        CameraRotation();
        Shake(_shouldShake);
    }

    private void Shake(bool value)
    {
        if(value == true)
        {
            if(_duration > 0)
            {
                _camera.transform.localPosition = _startPosition + Random.insideUnitSphere * _power;
                _duration -= Time.deltaTime * _slowDownAmount;
            }
            else
            {
                _shouldShake = false;
                _duration = _initialDuration;
                _camera.transform.localPosition = _startPosition;
            }
        }
    }

    private void CameraRotation()
    {
        _rotationX = _body.localEulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= InputManager.Instance.HorizontalSensitivity * Time.deltaTime;
        mouseY *= InputManager.Instance.VerticalSensitivity * Time.deltaTime;

        _currentX = Mathf.Lerp(_currentX, mouseX, _smoothTime * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, mouseY, _smoothTime * Time.deltaTime);

        _rotationX = _body.localEulerAngles.y + _currentX * InputManager.Instance.HorizontalSensitivity;

        _rotationY += _currentY * InputManager.Instance.VerticalSensitivity;
        _rotationY = Mathf.Clamp(_rotationY, -90, 90);

        _body.rotation = Quaternion.Euler(new Vector3(0, _rotationX, 0));
        _camera.transform.localRotation = Quaternion.Euler(-_rotationY, 0, 0);
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Camera -= OnUpdate;
            GameLoopManager.Instance.Pause -= IsPaused;
        }
    }
}
