using Engine.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsManager : Singleton<CheckPointsManager>
{
    private Vector3 _firstCheckPoint = Vector3.zero;
    private Vector3 _lastCheckpointPast = Vector3.zero;
    private List<CheckPoints> _checkPointList = null;
    private List<DeathZone> _deathZoneList = null;
    private bool _shouldRespawn = false;

    public List<CheckPoints> CheckPoints { get { return _checkPointList; } }
    public List<DeathZone> DeathZones { get { return _deathZoneList; } }

    private void Start()
    {
        _checkPointList = new List<CheckPoints>();
        _deathZoneList = new List<DeathZone>();
    }

    public void UpdateFirstCheckPoint(Vector3 pos)
    {
        _firstCheckPoint = pos;
    }

    public void AddCheckPoints(CheckPoints checkPoints)
    {
        _checkPointList.Add(checkPoints);
    }
    
    public void RemoveCheckPoints(CheckPoints checkPoints)
    {
        _checkPointList.Remove(checkPoints);
    }

    public void AddDeathZones(DeathZone deathZone)
    {
        _deathZoneList.Add(deathZone);
    }

    public void RemoveDeathZones(DeathZone deathZone)
    {
        _deathZoneList.Remove(deathZone);
    }

    public void ChangeCheckPoint(Vector3 pos)
    {
        _lastCheckpointPast = pos;
    }

    public void RespawnPlayer()
    {
        _shouldRespawn = true;
    }

    private void LateUpdate()
    {
        if(_shouldRespawn == true)
        {
            if (_lastCheckpointPast == Vector3.zero)
            {
                PlayerManager.Instance.Player.transform.position = _firstCheckPoint;
            }
            else
            {
                PlayerManager.Instance.Player.transform.position = _lastCheckpointPast;
            }

            _shouldRespawn = false;
        }
    }
}
