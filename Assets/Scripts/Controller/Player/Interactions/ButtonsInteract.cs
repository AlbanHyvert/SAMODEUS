using UnityEngine;

public class ButtonsInteract : MonoBehaviour, IAction, ICustomHighlight
{
    [SerializeField ,Header("HighLight Material")] private Material _highlightMat = null;
    [SerializeField ,Header("Other GO Action")] private GameObject _objectAction = null;
    private Renderer _rend = null;
    private Material _tempMat = null;
    private bool _setAction = false;

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _tempMat = _rend.sharedMaterial;
    }

    void ICustomHighlight.Enter()
    {
        _rend.sharedMaterial = _highlightMat;
    }

    void ICustomHighlight.Exit()
    {
        _rend.sharedMaterial = _tempMat;
    }

    void IAction.Enter()
    {
        _setAction = !_setAction;
        _objectAction.SetActive(_setAction);
    }

    void IAction.Exit()
    {
        _objectAction.SetActive(true);
    }
}
