using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeathZone : MonoBehaviour
{
    private void Start()
    {
        CheckPointsManager.Instance.AddDeathZones(this);

        Rigidbody rigidbody = transform.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if(playerController != null)
        {
            CheckPointsManager.Instance.RespawnPlayer(playerController.transform);
        }

        Pickable pickable = other.GetComponent<Pickable>();

        if(pickable != null)
        {
            Destroy(pickable.gameObject);
        }
    }

}
