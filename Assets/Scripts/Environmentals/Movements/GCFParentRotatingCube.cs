﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCFParentRotatingCube : MonoBehaviour
{
    [SerializeField] private Transform _orbitTarget = null;
    [SerializeField] private int _distanceMoveCube = 10;
    [SerializeField] private bool _isSpecialEvent = false;
    
    private float _distMin = 0f;
    private float _distMax = 0f;

    private List<GCFRotatingCube> _childList = null;

    public int DistMovingCube { get { return _distanceMoveCube; } set { _distanceMoveCube = value; } }
    private void Start()
    {
        _childList = new List<GCFRotatingCube>();
        _distMin = GCFManager.Instance.MinDistance;
        _distMax = GCFManager.Instance.MaxDistance;
        foreach (Transform item in transform)
        {
            _childList.Add(item.GetComponent<GCFRotatingCube>());
        }

        for (int i = 0; i < _childList.Count; i++)
        {
            if(_childList[i] != null)
            {
                _childList[i].transform.name = transform.name + '_' + "Child" + '_' + i;
                _childList[i].Init(_distMin, _distMax);
            }
        }
    }

    private void LateUpdate()
    {
        if(_isSpecialEvent == false)
        {
            if (PlayerManager.Instance.Player != null && _childList != null)
            {
                for (int i = 0; i < _childList.Count; i++)
                {
                    if (_childList[i] != null)
                    {
                        _childList[i].Orbit(PlayerManager.Instance.Player.transform.position, _orbitTarget, Vector3.forward);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < _childList.Count; i++)
            {
                if (_childList[i] != null)
                {
                    _childList[i].Orbit(_distanceMoveCube, _orbitTarget, Vector3.forward);
                }
            }
        }
    }
}
