using UnityEngine;

public class GCFRotatingCube : MonoBehaviour
{
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
            _distFromStartingPoint = Mathf.Lerp(_distFromStartingPoint, 0, _timeResetPos * _returnSpeedPosition);
        }
        else
        {
            _timeResetPos = 0;
            _timeResetPos += Time.deltaTime;
            if (_distFromStartingPoint < _tempDistFromStartPosition)
            {
                _distFromStartingPoint = Mathf.Lerp(_distFromStartingPoint, _tempDistFromStartPosition, _timeResetPos * _returnSpeedPosition);
            }
        }

        transform.position = _startPosition + Vector3.forward * Mathf.Sin(_timePass * _rotatingSpeed) * _distFromStartingPoint;
    }
}
