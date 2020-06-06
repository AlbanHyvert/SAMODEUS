using Engine.Loading;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyPortals : MonoBehaviour
{
    [SerializeField] private GameObject[] _dissolvingObject = null;
    [SerializeField] private string[] _scenesToUnload = null;
    [SerializeField] private float _speedDisolve = 1;
    [SerializeField] private float _maximalTimerValue = 10;

    private List<GameObject> _portals = null;
    private List<MeshRenderer> _plane = null;
    private bool _isActive = false;
    private float _timer = 0f;

    private void Start()
    {
        _portals = new List<GameObject>();
        _plane = new List<MeshRenderer>();

        if(PortalManager.Instance.PortalGCF != null && PortalManager.Instance.PortalVertumne != null)
        {
            _portals.Add(PortalManager.Instance.PortalVertumne.gameObject);
            _portals.Add(PortalManager.Instance.PortalGCF.gameObject);
            _plane.Add(PortalManager.Instance.PortalVertumne.MeshScreen);
            _plane.Add(PortalManager.Instance.PortalGCF.MeshScreen);
        }

        GameLoopManager.Instance.Puzzles += OnUpdate;
        _timer = 0.0f;

    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && _isActive == false)
        {
            _timer = 0.0f;
            _isActive = true;

            if(_portals != null)
            {
                for (int i = 0; i < _portals.Count; i++)
                {
                    Destroy(_portals[i].gameObject, 1);
                }
            }

            if (_plane != null)
            {
                for (int i = 0; i < _plane.Count; i++)
                {
                    Object.Destroy(_plane[i], 1);
                }
            }

            if(_scenesToUnload != null)
            {
                LoadingManager.Instance.UnloadScene(_scenesToUnload);
            }
        }
    }

    private void OnUpdate()
    {
        if(_isActive == true)
        {
            _timer += 0.01f * (_speedDisolve* Time.deltaTime);

            if (_dissolvingObject != null)
            {
                for (int i = 0; i < _dissolvingObject.Length; i++)
                {
                    if (_dissolvingObject[i] != null)
                    {
                        Renderer renderer = _dissolvingObject[i].GetComponent<Renderer>();

                        if (renderer != null)
                            renderer.material.SetFloat("Vector1_3996BBE4", _timer);
                    }
                }
            }

            if(_timer >= _maximalTimerValue)
            {
                for (int i = 0; i < _dissolvingObject.Length; i++)
                {
                    Object.Destroy(_dissolvingObject[i]);
                }
            }
        }
        else
        {
            _timer = 0.0f;
        }
    }
}
