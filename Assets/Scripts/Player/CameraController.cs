using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraStats _stats = null;
    [Space]
    [SerializeField] private Transform _body = null;
    [SerializeField] private Transform _hand = null;
    [Space]
    [SerializeField] private Camera _camera = null;
    [SerializeField] private HeadBobbing _headBobbing = null;

    #region PRIVATE
    private int _rotationXSpeed = 0;
    private int _rotationYSpeed = 0;
    private int _minClampXRotation = -70;
    private int _maxClampXRotation = 70;
    private float _rotationX = 0;
    private float _rotationY = 0;
    private float _currentX = 0;
    private float _currentY = 0;
    private float _initialDuration = 0.0f;
    private float _power = 1f;
    private float _duration = 1f;
    private float _slowDownAmount = 1f;
    private int _smoothTime = 5;
    private bool _shouldShake = false;
    private Vector3 _startPostion = Vector3.zero;
    #endregion PRIVATE

    public Transform Hand { get { return _hand; } }
    public Camera Camera { get { return _camera; } }

    public int RotationXSpeed { get { return _rotationXSpeed; } }
    public int RotationYSpeed { get { return _rotationYSpeed; } }

    private void Init()
    {
        _rotationXSpeed = _stats.RotationXSpeed;
        _rotationYSpeed = _stats.RotationYSpeed;

        _minClampXRotation = _stats.MinClampXRotation;
        _maxClampXRotation = _stats.MaxClampXRotation;

        InputManager.Instance.HorizontalSensitivity = _rotationXSpeed;
        InputManager.Instance.VerticalSensitivity = _rotationYSpeed;

        _power = _stats.Power;
        _duration = _stats.Duration;
        _slowDownAmount = _stats.SlowDownAmount;
        _smoothTime = _stats.SmoothTime;
        _headBobbing.Init(_camera, _stats.BobbingSpeed, _stats.BobbingForce, _stats.MidPoint, _stats.TimeMult);
    }

    private void Start()
    {
        Init();

        _startPostion = _camera.transform.localPosition;
        _initialDuration = _duration;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PlayerManager.Instance.CameraCanMove += CameraCanMove;

        GameLoopManager.Instance.Camera += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void CameraCanMove(bool value)
    {
        if(value == true)
        {
            GameLoopManager.Instance.Camera += OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.Camera -= OnUpdate;
        }
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
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

    private void Shake(bool value)
    {
        if (value == true)
        {
            if (_duration > 0)
            {
                _camera.transform.localPosition = _startPostion + Random.insideUnitSphere * _power;
                _duration -= Time.deltaTime * _slowDownAmount;
            }
            else
            {
                _shouldShake = false;
                _duration = _initialDuration;
                _camera.transform.localPosition = _startPostion;
            }
        }
    }

    private void OnUpdate()
    {
        _rotationYSpeed = InputManager.Instance.VerticalSensitivity;
        _rotationXSpeed = InputManager.Instance.HorizontalSensitivity;

        CameraRotation();
        Shake(_shouldShake);
    }

    private void CameraRotation()
    {
        _rotationX = _body.localEulerAngles.y;
        
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mouseX *= _rotationXSpeed / 10;
        mouseY *= _rotationYSpeed / 10;

        _currentX = Mathf.Lerp(_currentX, mouseX, _smoothTime * Time.deltaTime);
        _currentY = Mathf.Lerp(_currentY, mouseY, _smoothTime * Time.deltaTime);

        _rotationX = _body.localEulerAngles.y + _currentX * _rotationXSpeed;

        _rotationY += _currentY * _rotationYSpeed;
        _rotationY = Mathf.Clamp(_rotationY, _minClampXRotation, _maxClampXRotation);

        _body.rotation = Quaternion.Euler(new Vector3(0, _rotationX, 0));
        _camera.transform.localRotation = Quaternion.Euler(-_rotationY, 0, 0);

        _rotationX = _body.localEulerAngles.y;
    }

    private void OnDestroy()
    {
        if(PlayerManager.Instance !=null)
            PlayerManager.Instance.CameraCanMove -= CameraCanMove;

        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Camera -= OnUpdate;
            GameLoopManager.Instance.Pause -= IsPaused;
        }
    }
}
