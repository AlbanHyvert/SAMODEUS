using System.Collections.Generic;
using UnityEngine;

public class OnOffLight : MonoBehaviour
{
    [SerializeField] private double _range = 100;

    private List<GameObject> _childsList = null;

    private void Start()
    {
        _childsList = new List<GameObject>();

        foreach (Transform item in transform)
        {
            if(item != null)
            {
                _childsList.Add(item.gameObject);
            }
        }
    }

    private void Update()
    {
        Renderer renderer = transform.GetComponent<Renderer>();

        Vector3 playerPos = PlayerManager.Instance.Player.transform.position;

        double distFromPlayer = Vector3.Distance(playerPos, transform.position);

        if(distFromPlayer >= _range)
        {
            if(renderer != null)
            {
                renderer.enabled = false;
            }

            if(_childsList != null)
            {
                for (int i = 0; i < _childsList.Count; i++)
                {
                    if(_childsList[i] != null)
                    {
                        _childsList[i].SetActive(false);
                    }
                }
            }
        }
        else
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }

            if (_childsList != null)
            {
                for (int i = 0; i < _childsList.Count; i++)
                {
                    if (_childsList[i] != null)
                    {
                        _childsList[i].SetActive(true);
                    }
                }
            }
        }
    }
}
