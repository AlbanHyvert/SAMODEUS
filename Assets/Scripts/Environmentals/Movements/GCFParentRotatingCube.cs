using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCFParentRotatingCube : MonoBehaviour
{
    [SerializeField] private Transform _orbitTarget = null;
    [SerializeField] private int _distanceMoveCube = 10;
    [SerializeField] private bool _isSpecialEvent = false;

    private List<GCFRotatingCube> _rotatingChildList = null;
    private List<GCFMovingCube> _movingChildList = null;

    public int DistMovingCube { get { return _distanceMoveCube; } set { _distanceMoveCube = value; } }
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
        if(_isSpecialEvent == false)
        {
            Vector3 playerPosition = PlayerManager.Instance.Player.transform.position;

            if (PlayerManager.Instance.Player != null && _rotatingChildList != null)
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
                if (_rotatingChildList[i] != null)
                {
                    _rotatingChildList[i].Orbit(_distanceMoveCube, _orbitTarget, Vector3.forward);
                }
            }

            for (int i = 0; i < _movingChildList.Count; i++)
            {
                if (_movingChildList[i] != null)
                {
                    _movingChildList[i].MovingCube(_distanceMoveCube, GCFManager.Instance.DataGCFMovingCube.Speed);
                }
            }
        }
    }

    private void DestroyRotatingChild(GCFRotatingCube rotatingCube)
    {
        _rotatingChildList.Remove(rotatingCube);
    }

    private void DestroyMovingChild(GCFMovingCube movingCube)
    {
        _movingChildList.Remove(movingCube);
    }
}
