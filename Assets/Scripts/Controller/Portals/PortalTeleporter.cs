using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    private Transform _player = null;
    [SerializeField , Header("Teleportation Location")] Transform _receiver = null;
    private bool _playerIsOverlapping = false;

    private void Start()
    {
        if(_player == null)
        {
            _player = PlayerManager.Instance.Player.transform;
            if (_player == null)
            {
                throw new System.Exception("No Camera attach to the object. Exiting");
            }
        }
    }

    private void Update()
    {
        if (_playerIsOverlapping == true)
        {
            Vector3 portalToPlayer = _player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);

            if (dotProduct < 0f)
            {
                Teleport(_player, portalToPlayer);
                _playerIsOverlapping = false;
            }
        }
    }

    private void Teleport(Transform obj, Vector3 portalToObj)
    {   
            float rotationDiff = -Quaternion.Angle(transform.rotation, _receiver.rotation);
            rotationDiff += 180;
            obj.Rotate(Vector3.up, rotationDiff);

            Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToObj;
            obj.position = _receiver.position + positionOffset;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerIsOverlapping = false;
        }
    }
}
