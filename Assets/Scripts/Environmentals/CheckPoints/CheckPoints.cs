using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CheckPoints : MonoBehaviour
{
    [SerializeField] private bool _isFirstCheckPoint = false;

    private void Start()
    {
        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;

        CheckPointsManager.Instance.AddCheckPoints(this);

        if(_isFirstCheckPoint == true)
        {
            CheckPointsManager.Instance.UpdateFirstCheckPoint(transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if(playerController != null && _isFirstCheckPoint == false)
        {
            Debug.Log("checkpoint hit");
            CheckPointsManager.Instance.ChangeCheckPoint(transform);
        }
    }
}
