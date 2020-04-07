using UnityEngine;

public class GCFRotatingCube : MonoBehaviour
{
    private float _distFromStartingPoint = 0f;
    private float _resetPositionTimer = 0f;
    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation = Quaternion.identity;
    private float _offsetDiffFromPlayer = 0.0f;

    public float OffsetDiffFromPlayer { get { return _offsetDiffFromPlayer; } }

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    public void Init(float speedMin, float speedMax)
    {
        _distFromStartingPoint = Random.Range(speedMin, speedMax);
    }

    public void Orbit(Vector3 playerPosition, Transform target, Vector3 orientation)
    {
        _offsetDiffFromPlayer = Vector3.Distance(_startPosition, playerPosition);;

        if (_offsetDiffFromPlayer <= GCFManager.Instance.ActionRayonRotating)
        {
            _resetPositionTimer = 0;
            _resetPositionTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _startPosition, _resetPositionTimer * GCFManager.Instance.DataGCFRotatingCube.ReturnSpeed);
        }
        else
        {
            transform.RotateAround(target.position, orientation, _distFromStartingPoint * Time.deltaTime);
            transform.rotation = _startRotation;
        }
    }

    public void Orbit(float distance, Transform target, Vector3 orientation)
    {
        if (distance <= 0)
        {
            _resetPositionTimer = 0;
            _resetPositionTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, _startPosition, _resetPositionTimer * GCFManager.Instance.DataGCFRotatingCube.ReturnSpeed);
        }
        else
        {
            transform.RotateAround(target.position, orientation, _distFromStartingPoint * Time.deltaTime);
            transform.rotation = _startRotation;
        }
    }
}