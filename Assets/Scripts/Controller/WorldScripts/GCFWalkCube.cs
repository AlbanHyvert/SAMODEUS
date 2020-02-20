using UnityEngine;

public class GCFWalkCube : MonoBehaviour
{
    private float _speed = 4;
    private int _speedMult = 2;
    private float _speedMax = 10;
    private float _actionRayon = 10;

    private Vector3 _playerPosition = Vector3.zero;
    private Vector3 _startPosition = Vector3.zero;
    private Transform _object = null;
    private float _frequency = 1f;
    private float _timePass = 0f;

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

        Randomize();
        _object = this.transform;
        _startPosition = transform.position;
    }

    private void Update()
    {
        _playerPosition = PlayerManager.Instance.Player.transform.position;
        _timePass += Time.deltaTime * Time.timeScale * _frequency;
        float amplitude = Vector3.Distance(_object.position, _playerPosition);

        amplitude = Mathf.Clamp(amplitude, 0, 100);

        if (amplitude <= _actionRayon)
        {
            amplitude = 0;
        }

        transform.position = _startPosition + Vector3.down * Mathf.Sin(_timePass) * amplitude;
    }
}
