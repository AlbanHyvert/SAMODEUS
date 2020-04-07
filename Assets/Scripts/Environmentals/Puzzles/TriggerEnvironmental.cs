using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerEnvironmental : MonoBehaviour
{
    [SerializeField] private GCFParentControllerCube[] _activatedOBJ = null;

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
                for (int i = 0; i < _activatedOBJ.Length; i++)
                {
                    _activatedOBJ[i].DistMovingCube = 0;
                }
            }
        }
    }
}
