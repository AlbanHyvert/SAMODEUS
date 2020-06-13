using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region SERIALIZEFIELD
    [SerializeField, Header("Camera")] private CameraController _cameraController = null;
    [Space]
    [SerializeField, Header("Stats")] private PlayerStats _stats = null;
    [Space]
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicAudioSource = null;
    [SerializeField] private AudioSource _dialsAudioSource = null;
    [Space]
    [SerializeField, Header("Interaction Settings")] private InteractionStats _interactionStats = null;
    #endregion SERIALIZEFIELD

    #region PRIVATE
    private bool _handFull = false;
    private bool _isInteract = false;
    private float _speed = 5;
    private int _sprintMult;
    private int _sprintMinMult = 1;
    private int _sprintMaxMult = 2;
    private float _currentSpeed = 0;
    private float _smoothTime = 10f;
    private float _mass = 0;
    private float _gravity = 9.81f;
    private int _throwForce = 20;
    private int _maxDistanceInteractable = 10;
    private LayerMask _activeLayer = 0;
    private CharacterController _controller = null;
    private Transform _interactableObject = null;
    private WorldEnum _currentWorld = WorldEnum.VERTUMNE;
    #endregion PRIVATE

    #region PROPERTIES
    public PlayerStats Stats { get { return _stats; } }
    public AudioSource MusicAudioSource { get { return _musicAudioSource; } }
    public AudioSource DialsAudioSource { get { return _dialsAudioSource; } }
    public CameraController CameraController { get { return _cameraController; } }
    public float Speed { get { return _speed; } }
    public float Gravity { get { return _gravity; } }
    public bool IsInteract { get { return _isInteract; } set { IsInteractable(value); } }
    public bool HandFull { get { return _handFull; } }
    public WorldEnum CurrentWorld { get { return _currentWorld; } 
        set
        {
            SetCurrentWorld(value);
        }
    }
    #endregion PROPERTIES

    private void Awake()
    {
        DontDestroyOnLoad(this);

        PlayerManager.Instance.Player = this;
    }

    private void Start()
    {
        PlayerManager.Instance.ShouldMove += CanMove;
        PlayerManager.Instance.HaveGravity += UseGravity;

        GameLoopManager.Instance.Pause += IsPause;
        GameLoopManager.Instance.Player += OnUpdate;

        InputManager.Instance.Movement += OnMovements;
        InputManager.Instance.Idle += OnIdle;
        InputManager.Instance.PressSprint += OnSprint;
        InputManager.Instance.ReleaseSprint += OnWalk;
        InputManager.Instance.Gravity += OnGravity;

        float musicVolume = PlayerManager.Instance.MusicVolume / 100;
        float dialVolume = PlayerManager.Instance.DialVolume / 100;

        _dialsAudioSource.volume = dialVolume;
        _musicAudioSource.volume = musicVolume;

        Init();

        SetCurrentWorld(_currentWorld);
    }

    private void Init()
    {
        _controller = GetComponent<CharacterController>();

        _throwForce = _interactionStats.ThrowForce;
        _maxDistanceInteractable = _interactionStats.MaxDistanceInteract;
        _activeLayer = _interactionStats.ActiveLayer;

        _speed = _stats.Speed;
        _sprintMult = _stats.SprintMult;
        _sprintMinMult = _stats.SprintMinMult;
        _sprintMaxMult = _stats.SprintMaxMult;
        _mass = _stats.Mass;
        _gravity = _stats.Gravity;
    }

    private void IsPause(bool value)
    {
        if(value == true)
        {
            GameLoopManager.Instance.Player -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.Player += OnUpdate;
        }
    }

    private void CanMove(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.Movement += OnMovements;
            InputManager.Instance.PressSprint += OnSprint;
            InputManager.Instance.ReleaseSprint += OnWalk;
            InputManager.Instance.Gravity += OnGravity;
        }
        else
        {
            InputManager.Instance.Movement -= OnMovements;
            InputManager.Instance.PressSprint -= OnSprint;
            InputManager.Instance.ReleaseSprint -= OnWalk;
            InputManager.Instance.Gravity -= OnGravity;
        }
    }

    private void UseGravity(bool value)
    {
        if(value == true)
        {
            InputManager.Instance.Gravity += OnGravity;
            _controller.enabled = true;
        }
        else
        {
            InputManager.Instance.Gravity -= OnGravity;
            _controller.enabled = false;
        }
    }

    private void OnIdle()
    {
        _speed = 0;
        //HeadBobbing.OnIdle();
    }

    private void OnWalk()
    {
        _sprintMult = _sprintMinMult;
    }

    private void OnSprint()
    {
        _sprintMult = _sprintMaxMult;
    }

    private void OnMovements(Vector3 dir)
    {
        _speed = (_stats.Speed * _sprintMult);

        dir *= _currentSpeed / _mass;

        _controller.Move(dir * Time.deltaTime);
    }

    private void OnGravity(Vector3 dir)
    {
        dir.y -= _gravity;
        _controller.Move(dir * Time.deltaTime);
    }

    private void OnRaycast()
    {
        if(_handFull == false)
        {
            RaycastHit hit;

            bool isHit = Physics.Raycast(_cameraController.Camera.transform.position, _cameraController.Camera.transform.forward, out hit, _maxDistanceInteractable, _activeLayer);

            if (isHit != IsInteract)
            {
                IsInteract = isHit;

                if (IsInteract == true)
                {
                    _interactableObject = hit.transform;
                }
                else
                {
                    _interactableObject = null;
                }
            }
        }
    }

    private void OnUpdate()
    {
        _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, _smoothTime * Time.deltaTime);

        OnRaycast();
    }

    private void IsInteractable(bool value)
    {
        _isInteract = value;

        if(value == true)
        {
            InputManager.Instance.Interaction += OnPickUp;
            InputManager.Instance.Interaction += OnInteract;
        }
        else
        {
            InputManager.Instance.Interaction -= OnPickUp;
            InputManager.Instance.Interaction -= OnInteract;
        }
    }

    private void OnInteract()
    {
        IInteract interact = _interactableObject.GetComponent<IInteract>();

        if(_interactableObject != null && interact != null)
        {
            PlayerManager.Instance.PlayerCanMoveCamera = false;

            interact.Enter();

            _interactableObject = null;
        }

        PlayerManager.Instance.PlayerCanMoveCamera = true;
    }

    private void OnPickUp()
    {
        IAction action = _interactableObject.GetComponent<IAction>();

        if(_interactableObject != null && action != null)
        {
            PlayerManager.Instance.PlayerCanMoveCamera = false;

            action.Enter(this);
            
            _handFull = true;

            InputManager.Instance.Interaction += OnDrop;
            InputManager.Instance.Throw += OnThrow;
            InputManager.Instance.Interaction -= OnPickUp;
            InputManager.Instance.Interaction -= OnInteract;
        }
        PlayerManager.Instance.PlayerCanMoveCamera = true;
    }

    public void OnDrop()
    {
        PlayerManager.Instance.PlayerCanMoveCamera = false;

        IAction action = _interactableObject.GetComponent<IAction>();
        action.Exit();

        _handFull = false;

        PlayerManager.Instance.PlayerCanMoveCamera = true;

        _interactableObject = null;

        InputManager.Instance.Interaction -= OnDrop;
        InputManager.Instance.Throw -= OnThrow;
    }

    private void OnThrow()
    {
        PlayerManager.Instance.PlayerCanMoveCamera = false;

        IAction action = _interactableObject.GetComponent<IAction>();
        Rigidbody rigidbody = _interactableObject.GetComponent<Rigidbody>();

        action.Exit();
        rigidbody.AddForce(_cameraController.Camera.transform.forward * _throwForce, ForceMode.Impulse);
        
        _handFull = false;

        PlayerManager.Instance.PlayerCanMoveCamera = true;

        _interactableObject = null;

        InputManager.Instance.Interaction -= OnDrop;
        InputManager.Instance.Throw -= OnThrow;
    }

    private void SetCurrentWorld(WorldEnum world)
    {
        _currentWorld = world;
        PlayerManager.Instance.SetCurrentWorldMusic(_currentWorld);
    }

    private void OnDestroy()
    {
        if(PlayerManager.Instance != null)
        {
            PlayerManager.Instance.HaveGravity -= UseGravity;
            PlayerManager.Instance.ShouldMove -= CanMove;
            PlayerManager.Instance.Player = null;
        }

        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Pause -= IsPause;
            GameLoopManager.Instance.Player -= OnUpdate;
        }

        if(InputManager.Instance != null)
        {
            InputManager.Instance.Movement -= OnMovements;
            InputManager.Instance.Idle -= OnIdle;
            InputManager.Instance.PressSprint -= OnSprint;
            InputManager.Instance.ReleaseSprint -= OnWalk;
            InputManager.Instance.Gravity -= OnGravity;
        }
    }
}