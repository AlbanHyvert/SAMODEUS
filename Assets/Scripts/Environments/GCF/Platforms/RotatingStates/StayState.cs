using UnityEngine;

public class StayState : IPlateforms
{
    private RotatingCube _self = null;
    private Transform _target = null;
    private Transform _transform = null;
    private float _timePass = 0;
    private MeshRenderer _renderer = null;

    public void Enter()
    {
        // FX
        _renderer = _transform.GetComponent<MeshRenderer>();

        _renderer.enabled = true;

        if (_self.StayMaterial)
        {
            _renderer.material = _self.StayMaterial;
        }
        _timePass = 0;
    }

    public void Exit()
    {
        // other
        _timePass = 0;
    }

    public void Init(RotatingCube rotatingCube, Transform target)
    {
        _self = rotatingCube;
        _transform = rotatingCube.transform;
        _target = target;
    }

    public void Tick()
    {
        float DistFromPlayer = Vector3.Distance(_self.StartPosition, _self.Player.position);

        if(_self.OverrideState == true)
        {
            _timePass += 0.1f * Time.deltaTime;

            _transform.position = _self.StartPosition + Vector3.up * Mathf.Sin(_timePass * _self.StaySpeed) * _self.StayAmplitude;
        }
        else
        {
            if (DistFromPlayer >= _self.MaxDistFromPlayer)
            {
                _self.SetState(Plateforms_ENUM.MOVINGOUT);
            }
            else
            {
                _timePass += 0.1f * Time.deltaTime;

                _transform.position = _self.StartPosition + Vector3.up * Mathf.Sin(_timePass * _self.StaySpeed) * _self.StayAmplitude;
            }
        }
    }
}
