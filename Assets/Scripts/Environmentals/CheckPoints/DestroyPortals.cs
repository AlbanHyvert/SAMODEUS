using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyPortals : MonoBehaviour
{
    private List<Portal> _portals = null;
    private List<PortalNoScreen> _portalsNoScreen = null;
    private List<MeshRenderer> _plane = null;
    [SerializeField] private GameObject[] _gameObject = null;

    [SerializeField] private float _speedDisolve = 1;
    [SerializeField] private float _maximalTimerValue = 10;

    private bool _isActive = false;
    private float _timer = 0f;

    private void Start()
    {
        _portals = new List<Portal>();
        _portalsNoScreen = new List<PortalNoScreen>();
        _plane = new List<MeshRenderer>();

        _portals.Add(PortalManager.Instance.PortalVertumne);
        _portals.Add(PortalManager.Instance.PortalVertumne.LinkedPortal);
        _plane.Add(PortalManager.Instance.PortalVertumne.MeshScreen);
        _plane.Add(PortalManager.Instance.PortalGCF.MeshScreen);

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
                    Object.Destroy(_portals[i].gameObject, 1);
                }
            }

            if (_portalsNoScreen != null)
            {
                for (int i = 0; i < _portalsNoScreen.Count; i++)
                {
                    Object.Destroy(_portalsNoScreen[i].gameObject, 1);
                }
            }

            if (_plane != null)
            {
                for (int i = 0; i < _plane.Count; i++)
                {
                    Object.Destroy(_plane[i], 1);
                }
            }

            SceneAsyncManager.Instance.UnloadScene("Vertumne_1");
        }
    }

    private void OnUpdate()
    {
        if(_isActive == true)
        {
            _timer += 0.01f * (_speedDisolve* Time.deltaTime);

            if (_gameObject != null)
            {
                for (int i = 0; i < _gameObject.Length; i++)
                {
                    if (_gameObject[i] != null)
                    {
                        Renderer renderer = _gameObject[i].GetComponent<Renderer>();

                        if (renderer != null)
                            renderer.material.SetFloat("Vector1_3996BBE4", _timer);
                    }
                }
            }

            if(_timer >= _maximalTimerValue)
            {
                for (int i = 0; i < _gameObject.Length; i++)
                {
                    Object.Destroy(_gameObject[i]);
                }
            }
        }
        else
        {
            _timer = 0.0f;
        }
    }
}
