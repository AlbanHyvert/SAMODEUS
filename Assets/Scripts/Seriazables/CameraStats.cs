using UnityEngine;

[System.Serializable]
public class CameraStats
{
    [Header("Camera")]
    [SerializeField] private int _rotationXSpeed = 5;
    [SerializeField] private int _rotationYSpeed = 5;
    [SerializeField] private int _smoothTime = 5;
    [Space]
    [Header("HeadBobbing")]
    [SerializeField] private float _bobbingSpeed = 0.2f;
    [SerializeField] private float _bobbingForce = 0.2f;
    [Tooltip("For more stability should stay at : 0.0f")]
    [SerializeField] private float _midPoint = 0.0f;
    [SerializeField] private float _timeMult = 0.1f;
    [Space]
    [Header("Camera Shake Settings")]
    [SerializeField] private float _power = 1f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _slowDownAmount = 1f;

    public int RotationXSpeed { get { return _rotationXSpeed; } }
    public int RotationYSpeed { get { return _rotationYSpeed; } }
    public int SmoothTime { get { return _smoothTime; } }
    public float BobbingSpeed { get { return _bobbingSpeed; } }
    public float BobbingForce { get { return _bobbingForce; } }
    public float MidPoint { get { return _midPoint; } }
    public float TimeMult { get { return _timeMult; } }
    public float Power { get { return _power; } }
    public float Duration { get { return _duration; } }
    public float SlowDownAmount { get { return _slowDownAmount; } }
}
