using UnityEngine;

public class ResetTexture : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private bool _isPortal = false;
 
    private void OnTriggerEnter(Collider other)
    {
        Pickable pickable = other.GetComponent<Pickable>();

        if(_isPortal == false && pickable != null)
        {
            GetComponent<Renderer>().material = _material;
        }
        else if(_isPortal == true && pickable != null)
        {
            GetComponent<Renderer>().material = _material;
            GetComponent<Collider>().enabled = false;
        }
    }
}
