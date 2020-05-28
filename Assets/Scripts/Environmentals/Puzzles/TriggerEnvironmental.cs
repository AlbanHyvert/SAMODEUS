using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerEnvironmental : MonoBehaviour
{
    [SerializeField] private RotatingCube[] _activatedOBJ = null;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            if (_activatedOBJ != null)
            {
                // no idear
            }
        }
    }
}
