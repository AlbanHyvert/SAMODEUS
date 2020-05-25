using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IAction
{
    private Rigidbody _rigidbody = null;
    private FixedJoint _joint = null;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void IAction.Enter(PlayerController player)
    {
        //_rigidbody.transform.SetParent(obj);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _joint = player.CameraController.HandTransform.gameObject.AddComponent<FixedJoint>();
        _joint.connectedBody = _rigidbody;
    }

    void IAction.Exit()
    {
        _rigidbody.useGravity = true;
        Destroy(_joint);
    }

    void IAction.DestroySelf(Transform parent)
    {
        Destroy(_joint);
        _rigidbody.transform.SetParent(parent);
        transform.position = parent.position;
        _rigidbody.useGravity = true;
        Object.Destroy(this, 1);
    }
}
