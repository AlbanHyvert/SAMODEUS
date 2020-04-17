using System.Collections.Generic;
using UnityEngine;

public class GCFParentControllerCube : MonoBehaviour
{
    [SerializeField] private Transform _orbitTarget = null;
    [SerializeField] private int _distanceRotatingCube = 10;
    [SerializeField] private float _customEventAmplitude = 10;
    [SerializeField] private float _customEventSpeed = 10;
    [SerializeField] private bool _isSpecialEvent = false;

    private List<GCFRotatingCube> _rotatingChildList = null;
    private List<GCFMovingCube> _movingChildList = null;

    public int DistMovingCube { get { return _distanceRotatingCube; } set { _distanceRotatingCube = value; } }
    public float CustomEventAmplitude { get { return _customEventAmplitude; } set { _customEventAmplitude = value; } }
    public float CustomEventSpeed { get { return _customEventSpeed; } set { _customEventSpeed = value; } }

    private void Start()
    {
        _rotatingChildList = new List<GCFRotatingCube>();
        _movingChildList = new List<GCFMovingCube>();

        GameLoopManager.Instance.Puzzles += OnUpdate;

        foreach (Transform item in transform)
        {
            GCFRotatingCube rotatingCube = item.GetComponent<GCFRotatingCube>();
            GCFMovingCube movingCube = item.GetComponent<GCFMovingCube>();

            if (rotatingCube == null)
            {
                _rotatingChildList.Add(item.GetComponentInChildren<GCFRotatingCube>());
            }
            else
            {
                _rotatingChildList.Add(rotatingCube);
            }

            if (movingCube == null)
            {
                _movingChildList.Add(item.GetComponentInChildren<GCFMovingCube>());
            }
            else
            {
                _movingChildList.Add(movingCube);
            }
        }

        for (int i = 0; i < _rotatingChildList.Count; i++)
        {
            if(_rotatingChildList[i] != null)
            {
                _rotatingChildList[i].transform.name = transform.name + '_' + "Child" + '_' + i;
                _rotatingChildList[i].Init(GCFManager.Instance.DataGCFRotatingCube.MinDistance, GCFManager.Instance.DataGCFRotatingCube.MaxDistance);
            }

            if (_movingChildList[i] != null)
            {
                _movingChildList[i].transform.name = transform.name + '_' + "Child" + '_' + i;
                _movingChildList[i].Init(GCFManager.Instance.DataGCFMovingCube.MinAmplitude, GCFManager.Instance.DataGCFMovingCube.MaxAmplitude);
            }
        }
    }

    private void OnUpdate()
    {
        if(_isSpecialEvent == false && PlayerManager.Instance.Player != null)
        {
            Vector3 playerPosition = PlayerManager.Instance.Player.transform.position;

            if (PlayerManager.Instance.Player != null && _rotatingChildList != null && _orbitTarget != null)
            {
                for (int i = 0; i < _rotatingChildList.Count; i++)
                {
                    if (_rotatingChildList[i] != null)
                    {
                        _rotatingChildList[i].Orbit(playerPosition, _orbitTarget, Vector3.forward);
                    }
                }
            }

            if (PlayerManager.Instance.Player != null && _movingChildList != null)
            {
                for (int i = 0; i < _rotatingChildList.Count; i++)
                {
                    if (_movingChildList[i] != null)
                    {
                        _movingChildList[i].MovingCube(playerPosition, GCFManager.Instance.DataGCFMovingCube.Speed);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _rotatingChildList.Count; i++)
            {
                if (_rotatingChildList[i] != null && _orbitTarget != null)
                {
                    _rotatingChildList[i].Orbit(_distanceRotatingCube, _orbitTarget, Vector3.forward);
                }
            }

            for (int i = 0; i < _movingChildList.Count; i++)
            {
                if (_movingChildList[i] != null)
                {
                    _movingChildList[i].MovingCube(_distanceRotatingCube, _customEventAmplitude, _customEventSpeed);
                }
            }
        }
    }

    public void DestroyRotatingChild(GCFRotatingCube rotatingCube)
    {
        _rotatingChildList.Remove(rotatingCube);
    }

    public void DestroyMovingChild(GCFMovingCube movingCube)
    {
        _movingChildList.Remove(movingCube);
    }
}
