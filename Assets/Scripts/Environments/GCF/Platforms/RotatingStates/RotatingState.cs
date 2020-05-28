using UnityEngine;

public class RotatingState : IPlateforms
{
    private Transform _transform = null;
    private Transform _target = null;
    private RotatingCube _self = null;
    private float _time = 0;

    public void Enter()
    {
        //FX
        if (_self.RotatingMaterial)
        {
            MeshRenderer renderer = _transform.GetComponent<MeshRenderer>();
            renderer.material = _self.RotatingMaterial;
        }
        _time = 0;
    }

    public void Exit()
    {
        //other
        _time = 0;
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
            _self.SetState(Plateforms_ENUM.RETURN);
        }
        else
        {
            if (DistFromPlayer <= _self.MaxDistFromPlayer)
            {
                _self.SetState(Plateforms_ENUM.RETURN);
            }
            else
            {
                _time += _self.Speed * Time.deltaTime;

                _transform.RotateAround(_target.position, Vector3.forward, _self.Amplitude * _time);
            }
        }
    }
}
