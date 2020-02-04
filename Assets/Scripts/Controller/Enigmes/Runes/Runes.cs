using UnityEngine;

public class Runes : MonoBehaviour, IAction
{
    [SerializeField ,Header("Rotation Value")] private float _rotationValue = 1.1f;
    [SerializeField ,Header("Matching Material")] private Material _matchMat = null;
    [SerializeField ,Header("GO Material")] private GameObject _vfxMat = null;
    [SerializeField ,Header("Rotation Speed")] private float _rotationSpeed = 15f;

    private bool _isMathing = false;
    private float valueZ = 0;

    public void Start()
    {
        valueZ = transform.eulerAngles.z;
        GameLoopManager.Instance.Pause += Paused;
        GameLoopManager.Instance.GetInteractions += Rotation;
    }

    void IAction.Enter()
    {
        if(_isMathing == false)
        {
            valueZ += _rotationValue;
        }
    }

    private void Paused(bool value)
    {
        if (value == false)
        {
            GameLoopManager.Instance.GetInteractions += Rotation;
        }
        else
        {
            GameLoopManager.Instance.GetInteractions -= Rotation;
        }
    }

    private void Rotation()
    {
        Quaternion target_rotation = Quaternion.Euler(0, 0, valueZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, Time.deltaTime * _rotationSpeed);
    }

    void IAction.Exit()
    {
        _isMathing = true;
        _vfxMat.GetComponent<Renderer>().sharedMaterial = _matchMat;
        GameLoopManager.Instance.GetInteractions -= Rotation;
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.GetInteractions -= Rotation;
    }
}
