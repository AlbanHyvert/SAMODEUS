using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyPortals : MonoBehaviour
{
    [SerializeField] private Portal[] _portals = null;
    [SerializeField] private GameObject[] _plane = null;
    [SerializeField] private Renderer[] _renderer = null;
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null)
        {
            if(_portals != null)
            {
                for (int i = 0; i < _portals.Length; i++)
                {
                    Object.Destroy(_portals[i], 1);
                }
            }

            if(_plane != null)
            {
                for (int i = 0; i < _plane.Length; i++)
                {
                    Object.Destroy(_plane[i], 1);
                }
            }

            if(_renderer != null)
            {
                for (int i = 0; i < _renderer.Length; i++)
                {
                    GameObject obj = _renderer[i].GetComponent<GameObject>();

                    if(obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
    }
}
