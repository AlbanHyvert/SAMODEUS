using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IAction
{
    [SerializeField] private bool _shouldBeDestroyed = false;

    private bool _sceneQuitted = false;
    private Rigidbody _rigidbody = null;
    private FixedJoint _joint = null;
    private Vector3 _startPosition = Vector3.zero;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Vector3 StartPos { get { return _startPosition; } }

    private void Start()
    {
        Application.quitting += IsQuitting;
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
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

    private void OnDestroy()
    {
        if(_shouldBeDestroyed == false && CheckObjStatus.Instance != null)
        {
            if(_sceneQuitted == false && Application.isPlaying == true)
            {
                CheckObjStatus.Instance.RespawnDestroyedObj(this);
            }
        }
    }

    private void IsQuitting()
    {
        _sceneQuitted = true;
    }
}