using UnityEngine;

public class MovingOutState : IPlateforms
{
    private RotatingCube _self = null;
    private Transform _target = null;
    private Transform _transform = null;
    private Vector3 _extentedPosition = Vector3.zero;
    private float _time = 0;

    public void Enter()
    {
        //FX
        if(_self.MovingOutMaterial)
        {
            MeshRenderer renderer = _transform.GetComponent<MeshRenderer>();
            renderer.material = _self.MovingOutMaterial;
        }

        _time = 0;
        int x = Random.Range(0, 5);
        int y = Random.Range(0, 5);
        int z = Random.Range(0, 5);
        _extentedPosition = new Vector3(x,y,z) * _self.Amplitude;
    }

    public void Exit()
    {
        //FX and Sound
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
        float DistFromExtentedPosition = Vector3.Distance(_transform.position, _extentedPosition);
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
                if (DistFromExtentedPosition <= 0.2f)
                {
                    _self.SetState(Plateforms_ENUM.ROTATING);
                }
                else
                {
                    _time += 0.01f * Time.deltaTime;

                    _transform.position = Vector3.Lerp(_transform.position, _extentedPosition, _self.Speed * _time);
                    _transform.LookAt(_target);
                }
            }
        }
    }
}
