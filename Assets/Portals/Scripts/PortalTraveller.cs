using UnityEngine;

public class PortalTraveller : MonoBehaviour
{
    public Vector3 PreviousOffsetFromPortal { get; set; }

    public virtual void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        transform.rotation = rot;
        PlayerManager.Instance.Player.PlayerCamera.ShouldShake = true;
    }

    public virtual void EnterPortalThreshold()
    {

    }

    public virtual void ExitPortalThreshold()
    {

    }
}
