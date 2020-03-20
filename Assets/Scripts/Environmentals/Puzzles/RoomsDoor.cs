using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomsDoor : MonoBehaviour
{
    [SerializeField] private GameObject _door = null;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;

        if(_door != null)
        {
            _door.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if(player != null && _door != null)
        {
            _door.SetActive(true);
            Destroy(this);
        }

    }
}
