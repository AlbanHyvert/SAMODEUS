using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IAction
{
    private Rigidbody _rigidbody = null;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void IAction.Enter(Transform obj)
    {
        _rigidbody.transform.SetParent(obj);
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    void IAction.Exit()
    {
        _rigidbody.transform.SetParent(null);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    void IAction.DestroySelf(Transform parent)
    {
        _rigidbody.transform.SetParent(null);
        _rigidbody.transform.SetParent(parent);
        transform.position = parent.position;
        _rigidbody.useGravity = true;
        Object.Destroy(this, 1);
    }
}
