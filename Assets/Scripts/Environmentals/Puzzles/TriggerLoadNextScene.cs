using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerLoadNextScene : MonoBehaviour
{
    [SerializeField] private GameManager.GameState _ChoosenScene = GameManager.GameState.GAME;
    [SerializeField] private bool _specialItem = false;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        Pickable pickable = other.GetComponent<Pickable>();

        if(player != null)
        {
            GameManager.Instance.ChoosenScene = _ChoosenScene;
            GameManager.Instance.ChangeState(GameManager.GameState.LOADING);
        }

        if(pickable != null && _specialItem == true)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                transform.SetParent(null);
            }
        }
    }
}
