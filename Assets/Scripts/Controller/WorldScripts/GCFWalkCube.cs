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

    private void Randomize()
    {
        float rdmMinRange = Random.Range(_speed, _speed * _speedMult);
        float rdmMaxRange = Random.Range(rdmMinRange, _speedMax);

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
        _timePass += Time.deltaTime * _frequency;
        //Get the amplitude by the Start pos of the obj minus the one by the Player
        _amplitude = Vector3.Distance(_startPosition, _playerPosition);

        _amplitude = Mathf.Clamp(_amplitude, 0, 50);

        if (_amplitude <= _actionRayon)
        {
            _timeResetPos += Time.deltaTime;
            _amplitude = Mathf.Lerp(_amplitude, 0, _timeResetPos);
        }
        else
        {
            _timeResetPos = 0;
        }

        transform.position = _startPosition + Vector3.down * Mathf.Sin(_timePass) * _amplitude;
    }
}
