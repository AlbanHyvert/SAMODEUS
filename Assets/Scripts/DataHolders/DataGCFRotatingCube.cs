using UnityEngine;

[CreateAssetMenu(fileName = "DataGCFRotatingCube", menuName = "Data/DataGCFRotatingCube")]
public class DataGCFRotatingCube : ScriptableObject
{
    [SerializeField, Range(0, 4)] private float _speed = 0.2f;
    [SerializeField, Range(1, 10)] private float _returnSpeed = 1f;
    [SerializeField] private int _speedMult = 2;
    [SerializeField] private float _speedMax = 1f;

    [Header("Effect Range of the player")]
    [SerializeField] private float _actionRayon = 5f;

    [Header("Properties for Orbit")]
    [SerializeField] private float _maxDistance = 100;
    [SerializeField] private float _minDistance = 10;

    public float Speed { get { return _speed; } }
    public float ReturnSpeed { get { return _returnSpeed; } }
    public int SpeedMult { get { return _speedMult; } }
    public float SpeedMax { get { return _speedMax; } }
    public float ActionRayon { get { return _actionRayon; } }
    public float MaxDistance { get { return _maxDistance; } }
    public float MinDistance { get { return _minDistance; } }
}
