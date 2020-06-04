using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeathZone : MonoBehaviour
{
    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    private void OnTriggerStay(Collider other)
    {
        CharacterController player = other.transform.GetComponent<CharacterController>();
        Pickable pickable = other.transform.GetComponent<Pickable>();

        if(player != null)
        {
            PlayerManager.Instance.UseGravity = false;
            RespawnManager.Instance.Respawn(player);
        }

        if(pickable != null)
        {
            if(pickable.ShouldBeDestroyed == true)
            {
                Destroy(pickable.gameObject);
            }
            else
            {
                RespawnManager.Instance.ResetObjectPosition(pickable);
            }
        }
    }
}