using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IPlateforms
{
    private RotatingCube _self = null;
    private Transform _target = null;
    private Transform _transform = null;
    private float _timePass = 0;

    public void Enter()
    {
        // FX
        if (_self.ReturnMaterial)
        {
            MeshRenderer renderer = _transform.GetComponent<MeshRenderer>();
            renderer.material = _self.ReturnMaterial;
        }
        _timePass = 0;
    }

    public void Exit()
    {
        // FX and Sound
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
        float DistFromStartPosition = Vector3.Distance(_transform.position, _self.StartPosition);
        float DistFromPlayer = Vector3.Distance(_self.StartPosition, _self.Player.position);

        if(_self.OverrideState == true)
        {
            if (DistFromStartPosition <= 0.01f)
            {
                _self.SetState(Plateforms_ENUM.STAY);
            }
            else
            {
                _timePass += 0.01f * Time.deltaTime;

                _transform.position = Vector3.Lerp(_transform.position, _self.StartPosition, _self.ReturnSpeed * _timePass);
                _transform.rotation = _self.StartRotation;
            }
        }
        else
        {
            if (DistFromPlayer >= _self.MaxDistFromPlayer)
            {
                _self.SetState(Plateforms_ENUM.MOVINGOUT);
            }
            else
            {
                if (DistFromStartPosition <= 0.01f)
                {
                    _self.SetState(Plateforms_ENUM.STAY);
                }
                else
                {
                    _timePass += 0.01f * Time.deltaTime;

                    _transform.position = Vector3.Lerp(_transform.position, _self.StartPosition, _self.ReturnSpeed * _timePass);
                    _transform.rotation = _self.StartRotation;
                }
            }
        }
    }
}
