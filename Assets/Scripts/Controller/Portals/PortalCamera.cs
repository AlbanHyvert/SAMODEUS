using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    private Transform _playerCam = null;
    //Reference of the portal who is on at the same place of this Script
    [SerializeField , Header("Portal Position")] private Transform _portal = null;
    //Reference of the other portal position/rotation for camera position
    [SerializeField ,Header("Other Portal Position")] private Transform _otherPortal = null;
    private void Start()
    {
        if(_playerCam == null)
        {
            _playerCam = PlayerManager.Instance.Player.PlayerCamera.transform;
        }
    }
     
    private void Update()
    {
        //This will place the portal camera at the same place of the player camera postion
        Vector3 playerOffsetFromPortal = _playerCam.position - _otherPortal.position;
        transform.position = _portal.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortalRotation = Quaternion.Angle(_portal.rotation, _otherPortal.rotation);

        Quaternion portalRotationDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotation, Vector3.up);

        Vector3 newCameraDirection = portalRotationDifference * _playerCam.forward;
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
