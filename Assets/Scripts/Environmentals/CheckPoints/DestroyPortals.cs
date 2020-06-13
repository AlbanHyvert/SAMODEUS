using Engine.Loading;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyPortals : MonoBehaviour
{
    [SerializeField] private WorldEnum _newWorld = WorldEnum.GCF;
    [Space]
    [SerializeField] private MeshRenderer[] _dissolvingObject = null;
    [SerializeField] private string[] _scenesToUnload = null;
    [SerializeField, Range(-1, 1)] private float _startValueDissolve = 1;

    [Range(-1,1)] private float _dissolveValue = 1;
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

        _dissolveValue = _startValueDissolve;

        if (_dissolvingObject != null)
        {
            for (int i = 0; i < _dissolvingObject.Length; i++)
            {
                if (_dissolvingObject[i] != null)
                {
                    Material mat = _dissolvingObject[i].sharedMaterial;

                    mat.SetFloat("_Dissolve", _dissolveValue);
                }
            }
        }

        _timer = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && _isActive == false)
        {
            player.CurrentWorld = _newWorld;

            if (_portals != null)
            {
                for (int i = 0; i < _portals.Count; i++)
                {
                    Destroy(_portals[i].gameObject, 1);
                }
            }

            if (_scenesToUnload != null)
            {
                LoadingManager.Instance.UnloadScene(_scenesToUnload);
            }

            _timer = 0.0f;
            _isActive = true;

            GameLoopManager.Instance.Puzzles += OnUpdate;

            if (_plane != null)
            {
                for (int i = 0; i < _plane.Count; i++)
                {
                    Object.Destroy(_plane[i], 1);
                }
            }
        }
    }

    private void OnUpdate()
    {
        if(_isActive == true)
        {
            _timer = Time.deltaTime;

            if (_dissolvingObject != null)
            {
                _dissolveValue -= (0.5f * _timer);

                for (int i = 0; i < _dissolvingObject.Length; i++)
                {
                    if (_dissolvingObject[i] != null)
                    {
                        Material mat = _dissolvingObject[i].sharedMaterial;

                        mat.SetFloat("_Dissolve", _dissolveValue);
                    }
                }
            }

            if(_dissolveValue <= -0.1f)
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

    private void OnDestroy()
    {
        if (GameLoopManager.Instance != null)
            GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}