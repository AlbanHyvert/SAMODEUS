using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomsDoor : MonoBehaviour
{
    [SerializeField] private GameObject[] _door = null;
    [SerializeField] private GameObject[] _others = null;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;

        if(_door != null)
        {
            for (int i = 0; i < _door.Length; i++)
            {
                _door[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null)
        {
            if (_door != null)
            {
                for (int i = 0; i < _door.Length; i++)
                {
                    _door[i].SetActive(true);
                }
            }

            if(_others != null)
            {
                for (int j = 0; j < _others.Length; j++)
                {
                    Object.Destroy(_others[j], 1);
                }
            }
            Destroy(this);
        }
    }
}
