using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _smoothSpeed = 2;
    [Space]
    [SerializeField] private Transform _head = null;

    private bool _shouldTrackPlayer = false;
    private float x = 0;
    private float z = 0;

    private void Start()
    {
        if(_head == null)
        {
            Destroy(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            OnTrackPlayer(player);
        }
    }

    private void OnTrackPlayer(PlayerController player)
    {
        _head.LookAt(player.transform.position + Vector3.up * 2);
    }
}