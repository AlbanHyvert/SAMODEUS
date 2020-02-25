using Engine.Singleton;
using UnityEngine;

public class GCFManager : Singleton<GCFManager>
{
    [SerializeField, Range(0, 4)] private float _speed = 0.2f;
    [SerializeField, Range(0, 4)] private float _returnSpeed = 0.5f;
    [SerializeField] private int _speedMult = 2;
    [SerializeField] private float _speedMax = 1f;
    [SerializeField] private float _actionRayon = 5f;
    [SerializeField] private float _maxAmplitude = 100;
    [SerializeField] private float _minAmplitude = 10;
    public float Speed { get { return _speed; } }
    public float ReturnSpeed { get { return _returnSpeed; } }
    public int SpeedMult { get { return _speedMult; } }
    public float SpeedMax { get { return _speedMax; } }
    public float ActionRayon { get { return _actionRayon; } }
    public float MaxAmplitude { get { return _maxAmplitude; } }
    public float MinAmplitude { get { return _minAmplitude; } }
}
