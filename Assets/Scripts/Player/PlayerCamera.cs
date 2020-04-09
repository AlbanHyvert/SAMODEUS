using UnityEngine;
using SAMODEUS.Cameras;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private DataCamera _dataCamera = null;
    [SerializeField] private Transform _body = null;
    [SerializeField] private float _power = 0.7f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _slowDownAmount = 1f;
    [SerializeField] private Camera _camera = null;

    private float _rotationX = 0f;
    private float _rotationY = 0f;
    private bool _shouldShake = false;
    private Vector3 _startPosition = Vector3.zero;
    private float _initialDuration = 0.0f;

    public bool ShouldShake { get { return _shouldShake; } set { _shouldShake = value; } }
    public Camera Camera { get { return _camera; } }

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
        _rotationY = _body.eulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseX * InputManager.Instance.VerticalSensitivity;
        _rotationX -= mouseY * InputManager.Instance.HorizontalSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _dataCamera.MinimalVertRotation, _dataCamera.MaximalVertRotation);

        _camera.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        _body.eulerAngles = new Vector3(0, _rotationY, 0);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.Camera -= OnUpdate;
        GameLoopManager.Instance.Pause -= IsPaused;
    }
}
