using UnityEngine;

public class GCFMovingCube : MonoBehaviour
{
    private float _amplitude = 0f;
    private float _tempAmplitude = 0f;
    private Vector3 _playerPosition = Vector3.zero;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _movingSide = Vector3.zero;
    private Transform _object = null;
    private float _frequency = 1f;
    private float _timePass = 0f;
    private float _timeResetPos = 0f;
    private float _offsetDiffFromPlayer = 0.0f;

    public float OffsetDiffFromPlayer { get { return _offsetDiffFromPlayer; } }

    private void Randomize(float minAmplitude, float maxAmplitude)
    {
        float rdmAmplitude = Random.Range(minAmplitude, maxAmplitude);
        int chooseMove = Random.Range(1, 3);

        _amplitude = rdmAmplitude;
        _tempAmplitude = _amplitude;

        if(chooseMove <= 1)
        {
            _movingSide = Vector3.up;
        }
        else
        {
            _movingSide = Vector3.forward;
        }
    }

    public void Init(float minAmplitude, float maxAmplitude)
    {
        _amplitude = 0f;
        Randomize(minAmplitude, maxAmplitude);
        _object = this.transform;
        _startPosition = transform.position;
    }

    public void MovingCube(Vector3 playerPosition, float frequency)
    {
        // Set the time by the frequency
        _timePass += Time.deltaTime;

        //Get the amplitude by the Start pos of the obj minus the one by the Player
        _offsetDiffFromPlayer = Vector3.Distance(_startPosition, playerPosition);

        _amplitude = Mathf.Clamp(_amplitude, 0, GCFManager.Instance.DataGCFMovingCube.MaxAmplitude);

        // if the OffsetDiffPlayer is below or equal to the ActionRayon then move the block to their original position
        if (_offsetDiffFromPlayer <= GCFManager.Instance.ActionRayonMoving)
        {
            _timeResetPos = 0;
            _timeResetPos += Time.deltaTime;
            _amplitude = Mathf.Lerp(_amplitude, 0, _timeResetPos * GCFManager.Instance.DataGCFMovingCube.ReturnSpeed);
        }
        else
        {
            _timeResetPos = 0;
            _timeResetPos += Time.deltaTime;
            
            if(_amplitude < _tempAmplitude)
            {
                _amplitude = Mathf.Lerp(_amplitude, _tempAmplitude, _timeResetPos * GCFManager.Instance.DataGCFMovingCube.ReturnSpeed);
            }
        }

        transform.position = _startPosition + _movingSide * Mathf.Sin(_timePass * frequency) * _amplitude;
    }

    public void MovingCube(float distance, float amplitude, float frequency)
    {
       // Set the time by the frequency
        _timePass += Time.deltaTime;
    
        transform.position = _startPosition + _movingSide * Mathf.Sin(_timePass * frequency) * amplitude;
    }
}
