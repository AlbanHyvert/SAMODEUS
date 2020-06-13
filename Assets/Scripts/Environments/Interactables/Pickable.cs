using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickable : MonoBehaviour, IAction
{
    [SerializeField] private bool _shouldBeDestroyed = false;

    private Rigidbody _rigidbody = null;
    private FixedJoint _joint = null;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _tempScale = Vector3.zero;
    private float _speed = 6;
    private float _time = 0;
    private bool _hasAlreadyStarted = false;

    public Rigidbody Rigidbody { get { return _rigidbody; } }
    public Vector3 StartPos { get { return _startPosition; } }
    public bool ShouldBeDestroyed { get { return _shouldBeDestroyed; } }
    public bool HasAlreadyStarted { get { return _hasAlreadyStarted; } set { _hasAlreadyStarted = value; } }

    private void Start()
    {
        _tempScale = transform.localScale;
        transform.localScale = Vector3.zero;
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;

        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    private void OnUpdate()
    {
        _time = Time.deltaTime;

        transform.localScale = Vector3.Lerp(transform.localScale, _tempScale, _speed * _time);

        if(transform.localScale == _tempScale)
        {
            _time = 0;
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
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
        transform.rotation = parent.rotation;
        _rigidbody.useGravity = true;
        Destroy(this, 1);
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
            GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}