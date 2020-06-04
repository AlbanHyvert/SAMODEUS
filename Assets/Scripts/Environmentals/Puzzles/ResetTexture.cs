using UnityEngine;

public class ResetTexture : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private bool _isPortal = false;
    [SerializeField] private Room2 _room2 = null;

 
    private void OnTriggerEnter(Collider other)
    {
        Pickable pickable = other.GetComponent<Pickable>();

        if(pickable != null && _material != null)
        {
            if (!pickable.gameObject.tag.Equals("WrongObj"))
            {
                if (_isPortal == false && pickable != null)
                {
                    GetComponent<Renderer>().material = _material;
                }
                else if (_isPortal == true && pickable != null)
                {
                    GetComponent<Renderer>().material = _material;
                    GetComponent<Collider>().enabled = false;
                }
            }
            else
            {
                if(_room2 != null)
                    _room2.StartSecondEvent();
            }
        }
    }
}
