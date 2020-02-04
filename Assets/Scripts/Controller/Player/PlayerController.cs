using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("player Camera")] private Camera _playerCam = null;  
    [SerializeField, Header("Grounding Velocity"), Range(0, -10)] private float _minVelocity = -0.02f;
    [Header("Movement")]
    [SerializeField, Range(0, int.MaxValue)] private float _moveSpeed = 5f;
    [SerializeField, Range(0, int.MaxValue)] private float _sprintMax = 2f;
    [SerializeField, Range(0, int.MaxValue)] private float _jumpForce = 10f;
    [SerializeField, Header("Check Ground Distance"), Range(0, 0.7f)] private float _checkDistGround = 0.5f;
    [SerializeField, Header("Wet Duration"), Range(0, 10)] private float _wetTime = 0.5f;
    [SerializeField, Header("Interaction Controller")] private InteractionsController _interactController = null;
    [SerializeField, Header("RigidBoddy")] private Rigidbody _rb = null;
    [SerializeField, Header("Head Bobbing")] private HeadBobbing _headBobbing = null;
    [SerializeField, Header("Audio Source")] private AudioSource _source = null;

    private bool _isWet = false;
    private float _sprintMult = 1f;
    private float _timer = 0;
    private float _maxVelocity = 0f;
    #region Properties
    public Camera PlayerCamera { get { return _playerCam; } set { _playerCam = value; } }
    public float MinVelocity { get { return _minVelocity; } set { _minVelocity = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float SprintMax { get { return _sprintMax; } set { _sprintMax = value; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public float WetTime { get { return _wetTime; } set { _wetTime = value; } }
    public Rigidbody PlayerRigidbody { get { return _rb; } }
    public InteractionsController InteractionController { get { return _interactController; } }
    public AudioSource PlayerAudio { get { return _source; } }
    public bool IsWet { get { return _isWet; } }
    #endregion Properties

    private void Start()
    {
        GameLoopManager.Instance.GetInteractions += _interactController.OnUpdate;
        GameLoopManager.Instance.Pause += _interactController.IsPaused;

        InputManager.Instance.Move += Movement;
        InputManager.Instance.PressShift += Sprint;
        InputManager.Instance.ReleaseShift += Walk;
        InputManager.Instance.Jump += Jump;
        InputManager.Instance.Idle += Idle;
        GameLoopManager.Instance.GetPlayer += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;

        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody>();
            if(_rb == null)
            {
                throw new System.Exception(_rb + "PlayerController is trying to access a non existant Rigidbody. Exiting.");
            }
        }
    }

    private void Idle()
    {
        _headBobbing.OnIdle();
    }

    private void Movement(Vector3 direction)
    {
        _headBobbing.OnHeadBobbing(direction);
        _rb.AddForce(direction * _moveSpeed * _sprintMult, ForceMode.Force);
    }

    private void Jump(Vector3 direction)
    {
        if(CheckGround() == true)
        {
            _rb.AddForce(direction * _jumpForce, ForceMode.Impulse);
        }
    }

    private void Sprint()
    {
        _sprintMult = _sprintMax;
    }

    private void Walk()
    {
        float t = 0;
        t += 0.1f;
        _sprintMult = Mathf.Lerp(_sprintMax, 1, t);
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.GetPlayer -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.GetPlayer += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        if (_isWet == true && Time.time >= _timer)
        {
            _isWet = false;
        }
    }

    private bool CheckGround()
    {
        if (_rb.velocity.y > _minVelocity && _rb.velocity.y <= _maxVelocity)
        {
            return true;
        }
        else
            return false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("WaterPond"))
        {
            _isWet = true;
            _timer = Time.time + _wetTime;
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.Move -= Movement;
        InputManager.Instance.Jump -= Jump;
        InputManager.Instance.PressShift -= Sprint;
        InputManager.Instance.ReleaseShift -= Walk;
        GameLoopManager.Instance.GetPlayer -= OnUpdate;
        InputManager.Instance.Idle -= Idle;
    }
}