using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomsDoor : MonoBehaviour
{
    [SerializeField] private GameObject[] _door = null;

    private List<bool> _isActive = null;
    private void Start()
    {
        _isActive = new List<bool>();

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;

        if (_door != null)
        {
            for (int i = 0; i < _door.Length; i++)
            {
                _isActive.Add(_door[i].activeSelf);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (_door != null)
            {
                for (int i = 0; i < _door.Length; i++)
                {
                    _door[i].SetActive(!_isActive[i]);
                }
            }
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            _isActive.Clear();

            if (_door != null)
            {
                for (int i = 0; i < _door.Length; i++)
                {
                    _isActive.Add(_door[i].activeSelf);
                }
            }
        }
    }
}
