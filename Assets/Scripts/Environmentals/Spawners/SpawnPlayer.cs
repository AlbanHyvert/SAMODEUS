using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;

    private void Awake()
    {
        if(_camera != null)
        {
            _camera.enabled = false;
        }

        PlayerManager.Instance.CreatePlayer(transform.localPosition, transform.rotation);
    }
}
