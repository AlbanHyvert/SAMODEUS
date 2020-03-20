using UnityEngine;
using SAMODEUS.Cameras;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private DataCamera _dataCamera = null;
    [SerializeField] private Transform _body = null;

    private float _rotationX = 0f;
    private float _rotationY = 0f;

    private void Start()
    {
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
        _rotationY = _body.eulerAngles.y;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseX * InputManager.Instance.VerticalSensitivity;
        _rotationX -= mouseY * InputManager.Instance.HorizontalSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _dataCamera.MinimalVertRotation, _dataCamera.MaximalVertRotation);

        this.transform.localEulerAngles = new Vector3(_rotationX, 0, 0);
        _body.eulerAngles = new Vector3(0, _rotationY, 0);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.Camera -= OnUpdate;
        GameLoopManager.Instance.Pause -= IsPaused;
    }
}
