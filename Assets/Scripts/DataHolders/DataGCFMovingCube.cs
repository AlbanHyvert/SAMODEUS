using UnityEngine;

[CreateAssetMenu(fileName = "DataGCFMovingCube", menuName = "Data/DataGCFMovingCube")]
public class DataGCFMovingCube : ScriptableObject
{
    [SerializeField, Range(0, 4)] private float _speed = 0.2f;
    [SerializeField, Range(1, 10)] private float _returnSpeed = 1f;

    [Header("Effect Range of the player")]
    [SerializeField] private float _actionRayon = 5f;

    [Header("Properties for Moving")]
    [SerializeField] private float _maxAmplitude = 100;
    [SerializeField] private float _minAmplitude = 10;

    public float Speed { get { return _speed; } }
    public float ReturnSpeed { get { return _returnSpeed; } }
    public float ActionRayon { get { return _actionRayon; } }
    public float MaxAmplitude { get { return _maxAmplitude; } }
    public float MinAmplitude { get { return _minAmplitude; } }
}
