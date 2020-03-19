using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private void Awake()
    {
        PlayerManager.Instance.CreatePlayer(transform.localPosition, transform.rotation);
    }
}
