using System.Collections.Generic;
using UnityEngine;

public class ResetTextureRoom : MonoBehaviour
{
    [SerializeField] private Renderer[] _portalRenderer = null;
    [SerializeField] private Collider[] _colliders = null;

    private List<Material> _portalMatList = null;

    private void Start()
    {
        _portalMatList = new List<Material>();

        GameLoopManager.Instance.Puzzles += OnUpdate;

        for (int i = 0; i < _portalRenderer.Length; i++)
        {
            _portalMatList.Add(_portalRenderer[i].material);
        }
    }

    private void OnUpdate()
    {
        if(_portalMatList[0] != _portalRenderer[0].material && _portalMatList[1] != _portalRenderer[1].material)
        {
            Debug.Log("Correct");
            if(_colliders != null)
            {
                for (int i = 0; i < _colliders.Length; i++)
                {
                    _colliders[i].enabled = false;
                }
            }
        }
    }
}
