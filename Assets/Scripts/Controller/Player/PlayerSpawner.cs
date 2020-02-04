using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] Transform _spawnPos = null;

    void Awake()
    {
        PlayerManager.Instance.InstantiatePlayer(_spawnPos.position, _spawnPos.rotation);
    }
}
