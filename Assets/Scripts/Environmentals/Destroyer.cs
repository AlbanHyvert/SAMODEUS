using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Destroyer : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectToDestroy = null;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if(_objectToDestroy != null && playerController != null)
        {
            for (int i = 0; i < _objectToDestroy.Length; i++)
            {
                Destroy(_objectToDestroy[i]);
            }
        }
    }
}
