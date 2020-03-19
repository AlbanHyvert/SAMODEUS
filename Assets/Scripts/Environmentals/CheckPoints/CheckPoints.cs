using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    [Header("Starting Point")]
    [SerializeField] private Transform _firstCheckPoint = null;
    [Header("DeathZone")]
    [SerializeField] private DeathZone _respawnPoint = null;
    [Header("Place To Respawn")]
    [SerializeField] private Transform _placeToSpawn = null;

    private void Start()
    {
        if (_firstCheckPoint != null)
            _respawnPoint.SetCheckPoint(_firstCheckPoint.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            _respawnPoint.SetCheckPoint(_placeToSpawn.position);
        }
    }
}
