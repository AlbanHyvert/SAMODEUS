using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private GCFParentRotatingCube[] _activatedBridge = null;

    private List<BoxCollider> _collidersList = null;

    private void Start()
    {
        _collidersList = new List<BoxCollider>();

        if (_activatedBridge != null)
        {
            for (int i = 0; i < _activatedBridge.Length; i++)
            {
                for (int j = 0; j < _activatedBridge[i].GetComponents<BoxCollider>().Length; j++)
                {
                    _collidersList.Add(_activatedBridge[i].GetComponents<BoxCollider>()[j]);
                    _collidersList[j].enabled = false;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IAction action = other.GetComponent<IAction>();

        if (action != null && other.tag == _objectTag)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerController>().OnDrop();
            action.DestroySelf(_setPosition);

            if (_activatedBridge != null)
            {
                for (int i = 0; i < _activatedBridge.Length; i++)
                {
                    _activatedBridge[i].DistMovingCube = 0;
                    
                    if(_collidersList != null)
                    {
                        for (int j = 0; j < _activatedBridge[i].GetComponents<BoxCollider>().Length; j++)
                        {
                            _collidersList[j].enabled = true;
                        }
                    }

                }
            }
        }
    }
}
