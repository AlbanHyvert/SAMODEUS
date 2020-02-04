using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField ,Header("Renderer Material Array")] private Renderer[] _mat = null;
    [SerializeField ,Header("Activation Material")] private Material _active = null;
    [SerializeField ,Header("Object Place")] private Transform _objectPlace = null;

    private void OnTriggerEnter(Collider other)
    {
        Pickable obj = other.GetComponent<Pickable>();

        if(obj.tag.Equals("Diamond"))
        {
            obj.ActionExit();
            obj.ActionEnter(_objectPlace);
            obj.transform.position = _objectPlace.position;

            for (int i = 0; i < _mat.Length; i++)
            {
                _mat[i].sharedMaterial = _active;
            }
        }
    }
}
