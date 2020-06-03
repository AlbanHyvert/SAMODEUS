using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 PreviousOffsetFromPortal { get; set; }

    private bool _shouldTP = false;
    private Vector3 _position = Vector3.zero;
    private Quaternion _rotation = Quaternion.identity;
    private bool _shouldShake = false;
    private bool _canTP = true;
    private float _timer = 1;

    private void Start()
    {
        _canTP = true;
    }

    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, bool shakeValue)
    {
        if(_canTP == true)
        {
            _timer = 1;
            _shouldTP = true;
            _shouldShake = shakeValue;
            _position = pos;
            _rotation = rot;
        }

        /*transform.position = pos;
        transform.rotation = rot;
        PlayerManager.Instance.Player.CameraController.ShouldShake = shakeValue;*/
    }

    private void LateUpdate()
    {
        if(_canTP == true)
        {
            if (_shouldTP == true)
            {
                _canTP = false;
                transform.position = _position;
                transform.rotation = _rotation;
                PlayerManager.Instance.Player.CameraController.ShouldShake = _shouldShake;
                _shouldShake = false;
                _shouldTP = false;
            }

        }
        else
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _timer = 0;
                _canTP = true;
            }
        }
    }

    public virtual void EnterPortalThreshold()
    {

    }

    public virtual void ExitPortalThreshold()
    {

    }
}
