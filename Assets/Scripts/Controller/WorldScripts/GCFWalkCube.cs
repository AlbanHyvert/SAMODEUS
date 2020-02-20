using UnityEngine;

public class GCFWalkCube : MonoBehaviour
{
    private float _speed = 4;
    private int _speedMult = 2;
    private float _speedMax = 10;
    private float _actionRayon = 10;
    private float _amplitude = 0f;
    private Vector3 _playerPosition = Vector3.zero;
    private Vector3 _startPosition = Vector3.zero;
    private Transform _object = null;
    private float _frequency = 1f;
    private float _timePass = 0f;
    private float _timeResetPos = 0f;
    private bool _orientationMove = false;
    private void Randomize()
    {
        float rdmMinRange = Random.Range(_speed, _speed * _speedMult);
        float rdmMaxRange = Random.Range(rdmMinRange, _speedMax);
        int chooseMove = Random.Range(1, 3);

        if(chooseMove <= 1)
        {
            _orientationMove = false;
        }
        else
        {
            _orientationMove = true;
        }

        _frequency = Random.Range(rdmMinRange, rdmMaxRange);
    }

    private void Start()
    {
        _speed = GCFManager.Instance.Speed;
        _speedMult = GCFManager.Instance.SpeedMult;
        _speedMax = GCFManager.Instance.SpeedMax;
        _actionRayon = GCFManager.Instance.ActionRayon;
        _amplitude = 0f;
        Randomize();
        _object = this.transform;
        _startPosition = transform.position;
    }

    private void LateUpdate()
    {
        // Get Player Position
        _playerPosition = PlayerManager.Instance.Player.transform.position;
        // Set the time by the frequency
        _timePass += Time.deltaTime;
        //Get the amplitude by the Start pos of the obj minus the one by the Player
        float tempAmplitude = Vector3.Distance(_startPosition, _playerPosition);

        _amplitude = Mathf.Lerp(_amplitude, tempAmplitude, _timePass);

        _amplitude = Mathf.Clamp(_amplitude, 0, GCFManager.Instance.MaxAmplitude);

        if (_amplitude <= _actionRayon)
        {
            _timeResetPos += Time.deltaTime;
            _amplitude = Mathf.Lerp(_amplitude, 0, _timeResetPos);
        }
        else
        {
            _timeResetPos = 0;
        }

        if(_orientationMove == false)
        {
            transform.position = _startPosition + Vector3.forward * Mathf.Sin(_timePass * _frequency) * _amplitude;
        }
        else if(_orientationMove == true)
        {
            transform.position = _startPosition + Vector3.up * Mathf.Sin(_timePass * _frequency) * _amplitude;
        }

    }
}
