using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ResetTexture : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private Material _shader = null;
    [SerializeField] private LayerMask _layer;

    private Rigidbody _rigidBody = null;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Renderer renderer = other.GetComponent<Renderer>();
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (other.tag == "Puzzles" && renderer != null && playerController == null)
        {
            renderer.material = _shader;
        }
        else if (renderer != null && playerController == null)
        {
            renderer.material = _material;
        }
    }
}
