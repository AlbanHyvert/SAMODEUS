using UnityEngine;

public class GCFRotatingCube : MonoBehaviour
{
    private float _distFromStartingPoint = 0f;
    private float _distMin = 0f;
    private float _distMax = 0f;
    private float _resetPositionTimer = 0f;
    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation = Quaternion.identity;
    private Vector3 _playerPosition = Vector3.zero;

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public void Init(float speedMin, float speedMax)
    {
        _distMin = speedMin;
        _distMax = speedMax;
        _distFromStartingPoint = Random.Range(_distMin, _distMax);
    }

    public void Orbit(Vector3 playerPosition, Transform target, Vector3 orientation)
    {
        float OffsetDiffFromPlayer = Vector3.Distance(_startPosition, playerPosition);

        if (OffsetDiffFromPlayer <= GCFManager.Instance.ActionRayon)
        {
            _resetPositionTimer = 0;
            _resetPositionTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _startPosition, _resetPositionTimer * GCFManager.Instance.ReturnSpeed);
        }
        else
        {
            transform.RotateAround(target.position, orientation, _distFromStartingPoint * Time.deltaTime);
            transform.rotation = _startRotation;
        }
    }
}