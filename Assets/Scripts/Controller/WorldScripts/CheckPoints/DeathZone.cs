using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Vector3 _respawn = Vector3.zero;
    [SerializeField ,Header("First Apparition Point")] private Transform _firstRespawn = null;
    [SerializeField ,Header("Respawnable ?")] private bool _isRespawn = false;

    private void Start()
    {
        if(_respawn == Vector3.zero)
        {
            _respawn = _firstRespawn.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isRespawn == false)
        {
            if (other != null && !other.tag.Equals("Player"))
            {
                    Object.Destroy(other);
            }
        }

        else
        {
            if (other.tag.Equals("Player"))
            {
                other.transform.position = _respawn;
            }
        }
    }

    public void SetCheckPoint(Vector3 newPosition)
    {
        _respawn = newPosition;
    }
}
