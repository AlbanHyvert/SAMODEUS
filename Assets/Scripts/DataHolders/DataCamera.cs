using UnityEngine;

namespace SAMODEUS.Cameras
{
    [CreateAssetMenu(fileName = "DataCameras", menuName = "Data/Cameras")]
    public class DataCamera : ScriptableObject
    {
        [Header("Minimal & Maximal Y Rotation")]
        [SerializeField, Range(0, -360f)] private float _minimalVertRotation = -70f;
        [SerializeField, Range(0, 360f)] private float _maximalVertRotation = 70f;
        [SerializeField] private float _bobbingSpeed = 0.2f;
        [SerializeField] private float _bobbingAmount = 0.2f;
        [SerializeField] private float _midpoint = 0.0f;
        [SerializeField] private float _timeMult = 0.1f;

        public float MinimalVertRotation { get { return _minimalVertRotation; } }
        public float MaximalVertRotation { get { return _maximalVertRotation; } }
        public float BobbingSpeed { get { return _bobbingSpeed; } }
        public float BobbingAmount { get { return _bobbingAmount; } }
        public float MidPoint { get { return _midpoint; } }
        public float TimeMult { get { return _timeMult; } }
    }
}
