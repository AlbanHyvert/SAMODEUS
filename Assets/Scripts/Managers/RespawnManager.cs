using Engine.Singleton;
using UnityEngine;

public class RespawnManager : Singleton<RespawnManager>
{
    private Vector3 _firstCheckPoint = Vector3.zero;
    private Vector3 _lastCheckPoint = Vector3.zero;

    public Vector3 FirstRespawnPoint { get { return _firstCheckPoint; } set { SetFirstCheckPoint(value); } }
    public Vector3 LastRespawnPoint { get { return _lastCheckPoint; } set { SetLastCheckPoint(value); } }

    private void SetFirstCheckPoint(Vector3 pos)
    {
        _firstCheckPoint = pos;
    }

    private void SetLastCheckPoint(Vector3 pos)
    {
        _lastCheckPoint = pos;
    }

    public void Respawn(CharacterController player)
    {
        if(_lastCheckPoint == Vector3.zero)
        {
            player.transform.position = _firstCheckPoint;
        }
        else
        {
            player.transform.position = _lastCheckPoint;
        }

        PlayerManager.Instance.UseGravity = true;
    }

    public void ResetObjectPosition(Pickable pickable)
    {
        if (pickable.StartPos != Vector3.zero)
        {
            pickable.transform.position = pickable.StartPos;
        }
    }
}