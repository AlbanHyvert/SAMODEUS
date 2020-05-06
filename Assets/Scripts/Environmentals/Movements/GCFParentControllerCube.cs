using System.Collections.Generic;
using UnityEngine;

public class GCFParentControllerCube : MonoBehaviour
{
    [SerializeField] private Transform _orbitTarget = null;
    [SerializeField] private int _distanceRotatingCube = 10;
    [SerializeField] private float _customEventAmplitude = 10;
    [SerializeField] private float _customEventSpeed = 10;
    [SerializeField] private bool _isSpecialEvent = false;
    [SerializeField] private Renderer[] _renderersSpecialEvent = null;

    private List<Renderer> _rendererList = null;
    private List<GCFRotatingCube> _rotatingChildList = null;
    private List<GCFMovingCube> _movingChildList = null;

    public int DistMovingCube { get { return _distanceRotatingCube; } set { _distanceRotatingCube = value; } }
    public float CustomEventAmplitude { get { return _customEventAmplitude; } set { _customEventAmplitude = value; } }
    public float CustomEventSpeed { get { return _customEventSpeed; } set { _customEventSpeed = value; } }

    private void Start()
    {
        _rotatingChildList = new List<GCFRotatingCube>();
        _rendererList = new List<Renderer>();
        _movingChildList = new List<GCFMovingCube>();
        int value = 1;
        int ChoosenPieces = 2;
        GameLoopManager.Instance.Puzzles += OnUpdate;

        foreach (Transform item in transform)
        {
            int quantityOfNullObj = 0;
            GCFRotatingCube rotatingCube = item.GetComponent<GCFRotatingCube>();
            GCFMovingCube movingCube = item.GetComponent<GCFMovingCube>();
            Renderer renderer = item.GetComponent<Renderer>();

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

            if(value == ChoosenPieces)
            {
                ChoosenPieces += 2;

                if (rotatingCube == null)
                {
                    _rendererList.Add(item.GetComponentInChildren<Renderer>());
                }
                else
                {
                    _rendererList.Add(renderer);
                }

                if (movingCube == null)
                {
                    _rendererList.Add(item.GetComponentInChildren<Renderer>());
                }
                else
                {
                    _rendererList.Add(renderer);
                }
            }
            else
            {
                value++;
            }

            if(_rotatingChildList != null)
            {
                for (int i = 0; i < _rotatingChildList.Count; i++)
                {
                    if (_rotatingChildList[i] == null)
                    {
                        quantityOfNullObj++;
                    }
                }

                if (_rotatingChildList.Count <= quantityOfNullObj)
                {
                    _rotatingChildList.Clear();
                    quantityOfNullObj = 0;
                }
            }

            if(_movingChildList != null)
            {
                for (int i = 0; i < _movingChildList.Count; i++)
                {
                    if (_movingChildList[i] == null)
                    {
                        quantityOfNullObj++;
                    }
                }

                if (_movingChildList.Count <= quantityOfNullObj)
                {
                    _movingChildList.Clear();
                    quantityOfNullObj = 0;
                }
            }

        }

        if (_rotatingChildList != null)
        {
            for (int i = 0; i < _rotatingChildList.Count; i++)
            {
                if (_rotatingChildList[i] != null)
                {
                    _rotatingChildList[i].transform.name = transform.name + '_' + "Child" + '_' + i;
                    _rotatingChildList[i].Init(GCFManager.Instance.DataGCFRotatingCube.MinDistance, GCFManager.Instance.DataGCFRotatingCube.MaxDistance);
                }
            }
        }
        
        if(_movingChildList != null)
        {
            for (int i = 0; i < _movingChildList.Count; i++)
            {
                if (_movingChildList[i] != null)
                {
                    _movingChildList[i].transform.name = transform.name + '_' + "Child" + '_' + i;
                    _movingChildList[i].Init(GCFManager.Instance.DataGCFMovingCube.MinAmplitude, GCFManager.Instance.DataGCFMovingCube.MaxAmplitude);
                }
            }
        }
    }

    private void OnUpdate()
    {
        if(_isSpecialEvent == false && PlayerManager.Instance.Player != null)
        {
            Vector3 playerPosition = PlayerManager.Instance.Player.transform.position;

            if (_rotatingChildList != null && _orbitTarget != null)
            {
                for (int i = 0; i < _rotatingChildList.Count; i++)
                {
                    if (_rotatingChildList[i] != null)
                    {
                        _rotatingChildList[i].Orbit(playerPosition, _orbitTarget, Vector3.forward);
                    }
                }
            }

            if (_movingChildList != null)
            {
                for (int i = 0; i < _movingChildList.Count; i++)
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

        if(_rendererList != null)
        {
            IsRendered();
        }

        if(_renderersSpecialEvent != null)
        {
            IsRenderedSpecialEvent();
        }
    }

    private void IsRendered()
    {
        for (int i = 0; i < _rendererList.Count; i++)
        {
            if (_rendererList[i] != null)
            {
                GCFRotatingCube gCFRotatingCube = _rendererList[i].GetComponent<GCFRotatingCube>();
                GCFMovingCube gCFMovingCube = _rendererList[i].GetComponent<GCFMovingCube>();

                if(gCFMovingCube != null)
                {
                    if(gCFMovingCube.OffsetDiffFromPlayer <= GCFManager.Instance.ActionRayonMoving)
                    {
                        _rendererList[i].enabled = true;
                    }
                    else
                    {
                        _rendererList[i].enabled = false;
                    }
                }

                if(gCFRotatingCube != null)
                {
                    if(gCFRotatingCube.OffsetDiffFromPlayer <= GCFManager.Instance.ActionRayonRotating)
                    {
                        _rendererList[i].enabled = true;
                    }
                    else
                    {
                        _rendererList[i].enabled = false;
                    }
                }
            }
        }
    }

    private void IsRenderedSpecialEvent()
    {
        if (_distanceRotatingCube <= 0)
        {
            for (int i = 0; i < _renderersSpecialEvent.Length; i++)
            {
                if(_renderersSpecialEvent[i] != null)
                    _renderersSpecialEvent[i].enabled = true;
            }
        }
        else if (_distanceRotatingCube > 0)
        {
            for (int i = 0; i < _renderersSpecialEvent.Length; i++)
            {
                if (_renderersSpecialEvent[i] != null)
                    _renderersSpecialEvent[i].enabled = false;
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
