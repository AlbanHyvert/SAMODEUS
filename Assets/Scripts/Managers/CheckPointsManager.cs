using Engine.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointsManager : Singleton<CheckPointsManager>
{
    private Transform _firstCheckPoint = null;
    private Transform _lastCheckpointPast = null;
    private List<CheckPoints> _checkPointList = null;
    private List<DeathZone> _deathZoneList = null;

    public List<CheckPoints> CheckPoints { get { return _checkPointList; } }
    public List<DeathZone> DeathZones { get { return _deathZoneList; } }
    private void Start()
    {
        _checkPointList = new List<CheckPoints>();
        _deathZoneList = new List<DeathZone>();
    }

    public void UpdateFirstCheckPoint(Transform transform)
    {
        _firstCheckPoint = transform;
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

    public void ChangeCheckPoint(Transform transform)
    {
        _lastCheckpointPast = transform;
    }

    public void RespawnPlayer(Transform player)
    {
        if(_lastCheckpointPast == null)
        {
            player.position = _firstCheckPoint.position;
            player.rotation = _firstCheckPoint.rotation;
        }
        else
        {
            player.position = _lastCheckpointPast.position;
            player.rotation = _lastCheckpointPast.rotation;
        }

        Debug.Log("RespawnPlayer");
    }
}
