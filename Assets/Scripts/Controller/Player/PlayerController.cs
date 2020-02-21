using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region FIELDS
    [SerializeField, Header("player Camera")] private Camera _playerCam = null;
    [Header("Movement")]
    [SerializeField, Range(0f, 1000f)] private float _moveSpeed = 5f;
    [SerializeField, Range(0f, 1000f)] private float _sprintMax = 2f;
    [SerializeField, Range(0f, 1000f)] private float _jumpForce = 10f;
    [SerializeField, Range(0f, 1000f)] private float _gravity = 9.81f;
    [SerializeField, Header("Wet Duration"), Range(0, 10)] private float _wetTime = 0.5f;
    [SerializeField, Header("Interaction Controller")] private InteractionsController _interactController = null;
    [SerializeField] private CharacterController controller = null;
    [SerializeField, Header("Head Bobbing")] private HeadBobbing _headBobbing = null;
    [SerializeField, Header("Audio Source")] private AudioSource _source = null;

    private bool _isWet = false;
    private float _sprintMult = 1f;
    private float _timer = 0;
    private float _maxVelocity = 0f;
    private Vector3 _playerDir = Vector3.zero;
    #endregion FIELDS

    #region PROPERTIES
    public Camera PlayerCamera { get { return _playerCam; } set { _playerCam = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public float SprintMax { get { return _sprintMax; } set { _sprintMax = value; } }
    public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }
    public float WetTime { get { return _wetTime; } set { _wetTime = value; } }
    public InteractionsController InteractionController { get { return _interactController; } }
    public AudioSource PlayerAudio { get { return _source; } }
    public bool IsWet { get { return _isWet; } }
    #endregion PROPERTIES

    #region METHODS
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        GameLoopManager.Instance.UpdateInteractions += _interactController.OnUpdate;
        GameLoopManager.Instance.Pause += _interactController.IsPaused;

        InputManager.Instance.Move += Movement;
        InputManager.Instance.PressShift += Sprint;
        InputManager.Instance.ReleaseShift += Walk;
        InputManager.Instance.Jump += Movement;
        InputManager.Instance.Idle += Idle;
        GameLoopManager.Instance.UpdatePlayer += OnUpdate;
        GameLoopManager.Instance.UpdateGravity += Gravity;
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void Idle()
    {
        _headBobbing.OnIdle();
    }

    private void Movement(Vector3 direction)
    {
        direction *= _moveSpeed * _sprintMult;
        _headBobbing.OnHeadBobbing(direction);
        controller.Move(direction * Time.deltaTime);
    }

    private void Gravity(Vector3 dir)
    {
        dir.y -= _gravity;

        controller.Move(dir * Time.deltaTime);
    }

    private void Sprint()
    {
        _sprintMult = _sprintMax;
    }

    private void Walk()
    {
        _sprintMult = 1;
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.UpdatePlayer -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.UpdatePlayer += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        if (_isWet == true && Time.time >= _timer)
        {
            _isWet = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("WaterPond"))
        {
            _isWet = true;
            _timer = Time.time + _wetTime;
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.Move -= Movement;
        InputManager.Instance.PressShift -= Sprint;
        InputManager.Instance.ReleaseShift -= Walk;
        GameLoopManager.Instance.UpdatePlayer -= OnUpdate;
        InputManager.Instance.Idle -= Idle;
    }
    #endregion METHODS
}