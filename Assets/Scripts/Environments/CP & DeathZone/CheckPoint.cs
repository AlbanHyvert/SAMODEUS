using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private bool _isFirstWorldRespawn = false;

    private bool _hasAlreadyBeenUsed = false;

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
    }

    private void Start()
    {
        if(_isFirstWorldRespawn == true)
        {
            RespawnManager.Instance.FirstRespawnPoint = transform.position;
            _hasAlreadyBeenUsed = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.transform.GetComponent<PlayerController>();

        if(_hasAlreadyBeenUsed == false && player != null)
        {
            RespawnManager.Instance.LastRespawnPoint = transform.position;
            _hasAlreadyBeenUsed = true;
        }
    }
}