using Engine.Singleton;
using UnityEngine;

public class GCFManager : Singleton<GCFManager>
{
    [SerializeField, Range(0, 4)] private float _speed = 0.2f;
    [SerializeField] private int _speedMult = 2;
    [SerializeField] private float _speedMax = 1f;
    [SerializeField] private float _actionRayon = 5f;

    public float Speed { get { return _speed; } }
    public int SpeedMult { get { return _speedMult; } }
    public float SpeedMax { get { return _speedMax; } }
    public float ActionRayon { get { return _actionRayon; } }
}
