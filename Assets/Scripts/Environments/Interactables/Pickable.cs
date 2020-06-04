using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IAction
{
    [SerializeField] private bool _shouldBeDestroyed = false;

    private Rigidbody _rigidbody = null;
    private FixedJoint _joint = null;
    private Vector3 _startPosition = Vector3.zero;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Vector3 StartPos { get { return _startPosition; } }
    public bool ShouldBeDestroyed { get { return _shouldBeDestroyed; } }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
    }

    void IAction.Enter(PlayerController player)
    {
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _joint = player.CameraController.Hand.gameObject.AddComponent<FixedJoint>();
        transform.SetParent(player.CameraController.Camera.transform);
        _joint.connectedBody = _rigidbody;
    }

    void IAction.Exit()
    {
        _rigidbody.useGravity = true;
        transform.SetParent(null);
        Destroy(_joint);
    }

    void IAction.DestroySelf(Transform parent)
    {
        Destroy(_joint);
        _rigidbody.transform.SetParent(parent);
        transform.position = parent.position;
        _rigidbody.useGravity = true;
        Destroy(this, 1);
    }
}