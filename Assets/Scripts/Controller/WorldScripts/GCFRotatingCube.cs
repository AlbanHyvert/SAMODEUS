using UnityEngine;

public class GCFRotatingCube : MonoBehaviour
{
    [SerializeField] private Transform _orbitTarget = null;
    private Quaternion _startRotation = Quaternion.identity;
    private float _rotatingSpeed = 0;
    private float _distFromStartingPoint = 0f;
    private float _tempDistFromStartPosition = 0f;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _playerPosition = Vector3.zero;
    private float _timePass = 0f;
    private float _timeResetPos = 0f;
    private float _returnSpeedPosition = 2f;
    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _rotatingSpeed = GCFManager.Instance.Speed;
        _distFromStartingPoint = (int)Random.Range(GCFManager.Instance.MinAmplitude, GCFManager.Instance.MaxAmplitude);
        _tempDistFromStartPosition = _distFromStartingPoint;
    }

    private void LateUpdate()
    {
        _playerPosition = PlayerManager.Instance.Player.transform.position;

        _timePass += Time.deltaTime;

        float OffsetDiffFromPlayer = Vector3.Distance(_startPosition, _playerPosition);

        if (OffsetDiffFromPlayer <= GCFManager.Instance.ActionRayon)
        {
            _timeResetPos = 0;
            _timeResetPos += Time.deltaTime;
            transform.position = _startPosition;
        }
        else
        {
            transform.RotateAround(_orbitTarget.position, Vector3.forward, _distFromStartingPoint * Time.deltaTime);
            transform.rotation = _startRotation;
        }


    }
}
