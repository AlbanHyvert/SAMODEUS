using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField, Header("Player RigidBody")] private Rigidbody _playerRb = null;
    [SerializeField ,Header("Player Camera")] private Camera _cam = null;

    [Header("Min & Max y Rotation"), Range(0,-360)]
    [SerializeField] private float _minVert = -45f;
    [Range(0,360)]
    [SerializeField] private float _maxVert = 45f;
    private float _rotationX = 0f;

    private void Start()
    {
        GameLoopManager.Instance.GetCamera += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
        if(_playerRb == null)
        {
            throw new System.Exception("CameraController is trying to access a non-existant object" + _playerRb + "Exiting");
        }

        if (_cam == null)
        {
            _cam = GetComponentInChildren<Camera>();
            if(_cam == null)
            {
                throw new System.Exception("CameraController is trying to access a non-existant object" + _cam + "Exiting");
            }
        }
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.GetCamera -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.GetCamera += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _playerRb.transform.Rotate(0, mouseX * InputFieldManager.Instance.HorizontalSensivity, 0);
        _rotationX -= mouseY * InputFieldManager.Instance.VerticalSensivity;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);

        float rotationY = _playerRb.transform.localEulerAngles.y;

        _cam.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        _playerRb.transform.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.GetCamera -= OnUpdate;
    }
}
