using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour
{
    [SerializeField, Header("HighLight Material")] private Material _highlightMat = null;
    private InteractionsController _tempInteractionController = null;
    private Renderer _rend = null;
    private Material _tempMat = null;
    private bool _isPick = false;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _tempMat = _rend.sharedMaterial;
    }

    public void CustomHighlightEnter()
    {
        if(_isPick == false)
        {
            _rend.sharedMaterial = _highlightMat;
        }
    }

    public void CustomHighlightExit()
    {
        _rend.sharedMaterial = _tempMat;
    }

    public void GetPlayer(InteractionsController player)
    {
        _tempInteractionController = player;
    }

   public void ActionEnter(Transform obj)
    {
        this.transform.SetParent(obj);
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = true;
        _isPick = true;
        _rend.sharedMaterial = _tempMat;
    }

    public void ActionExit()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        if(_tempInteractionController != null)
        {
            _tempInteractionController.Drop();
        }
        _tempInteractionController = null;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        this.transform.SetParent(null);
        _isPick = false;
        //_rend.sharedMaterial = _tempMat;
    }
}
