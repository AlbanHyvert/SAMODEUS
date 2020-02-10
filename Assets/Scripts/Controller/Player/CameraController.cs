using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField, Header("Player RigidBody")] private Transform _player = null;
    [SerializeField ,Header("Player Camera")] private Camera _cam = null;

    [Header("Min & Max y Rotation"), Range(0,-360)]
    [SerializeField] private float _minVert = -45f;
    [Range(0,360)]
    [SerializeField] private float _maxVert = 45f;
    private float _rotationX = 0f;

    private void Start()
    {
        GameLoopManager.Instance.UpdateCamera += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;

        if (_cam == null)
        {
            _cam = GetComponentInChildren<Camera>();
        }
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.UpdateCamera -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.UpdateCamera += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _player.Rotate(0, mouseX * InputManager.Instance.HoriSensivity, 0);
        _rotationX -= mouseY * InputManager.Instance.VertSensivity;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);

        float rotationY = _player.localEulerAngles.y;

        _cam.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        _player.localEulerAngles = new Vector3(0, rotationY, 0);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.UpdateCamera -= OnUpdate;
    }
}
