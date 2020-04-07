using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ChangeStatInGCFManager : MonoBehaviour
{
    [SerializeField] private float _newActionRayon = 10;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if(playerController != null)
        {
            GCFManager.Instance.ActionRayonMoving = _newActionRayon;
            GCFManager.Instance.ActionRayonRotating = _newActionRayon;
        }
    }
}