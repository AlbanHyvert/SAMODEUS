using System.Collections.Generic;
using UnityEngine;

public class RotatingCube : MonoBehaviour
{
    #region SERIALIZEFIELD
    [Header("Materials Per State")]
    [SerializeField] private Material _stayMaterial = null;
    [SerializeField] private Material _movingOutMaterial = null;
    [SerializeField] private Material _returnMaterial = null;
    [SerializeField] private Material _rotatingMaterial = null;
    [Space]
    [SerializeField] private Plateforms_ENUM _currentState = Plateforms_ENUM.STAY;
    [Space]
    [Header("Orbit Target")]
    [SerializeField] private Transform _target = null;
    [Space]
    [SerializeField] private float _maxDistanceFromPlayer = 20;
    [SerializeField] private bool _shouldRender = true;
    [Space]
    [Header("Max and Min Amplitude")]
    [SerializeField] private float _maxAmplitude = 30;
    [SerializeField] private float _minAmplitude = 10;
    [Header("StayIn Amplitude")]
    [SerializeField] private float _stayAmplitude = 1.5f;
    [Space]
    [Header("Max and Min Speed")]
    [SerializeField] private float _maxSpeed = 30;
    [SerializeField] private float _minSpeed = 10;
    [Space]
    [Header("Speed When MovingOut")]
    [SerializeField] private float _movingOutSpeed = 10;
    [Header("Return Speed")]
    [SerializeField] private float _returnSpeed = 20;
    [Header("Stay In Place Speed")]
    [SerializeField] private float _staySpeed = 2;
    [Space]
    [Header("Is A Special Event")]
    [SerializeField] private bool _isSpecialEvent = false;
    #endregion SERIALIZEFIELD

    private bool _overrideState = false;
    private float _amplitude = 0;
    private float _speed = 0;
    private Dictionary<Plateforms_ENUM, IPlateforms> _states = null;
    private Transform _player = null;
    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation = Quaternion.identity;
    private MeshRenderer _renderer = null;
    private bool _tempRendererEnabled = false;
    private bool changeRendererValue = false;
    private Transform _portal = null;

    #region PROPERTIES
    public bool IsSpecialEvent { get { return _isSpecialEvent; } }
    public bool OverrideState { get { return _overrideState; } set { SetOverrideState(value); } }
    public Material StayMaterial { get { return _stayMaterial; } }
    public Material MovingOutMaterial { get { return _movingOutMaterial; } }
    public Material ReturnMaterial { get { return _returnMaterial; } }
    public Material RotatingMaterial { get { return _rotatingMaterial; } }
    public Transform Player { get { return _player; } }
    public float MaxDistFromPlayer { get { return _maxDistanceFromPlayer; } }
    public bool ShouldRender { get { return _shouldRender; } }
    public float Amplitude { get { return _amplitude; } }
    public float StayAmplitude { get { return _stayAmplitude; } }
    public float Speed { get { return _speed; } }
    public float MovingOutSpeed { get { return _movingOutSpeed; } }
    public float ReturnSpeed { get { return _returnSpeed; } }
    public float StaySpeed { get { return _staySpeed; } }
    public Vector3 StartPosition { get { return _startPosition; } }
    public Quaternion StartRotation { get { return _startRotation; } }
    #endregion PROPERTIES

    private void Init()
    {
        Plateforms_ENUM tempState = _currentState;

        _states = new Dictionary<Plateforms_ENUM, IPlateforms>();

        _states.Add(Plateforms_ENUM.STAY, new StayState());
        _states.Add(Plateforms_ENUM.MOVINGOUT, new MovingOutState());
        _states.Add(Plateforms_ENUM.ROTATING, new RotatingState());
        _states.Add(Plateforms_ENUM.RETURN, new ReturnState());

        _currentState = tempState;

        _states[Plateforms_ENUM.STAY].Init(this, _target);
        _states[Plateforms_ENUM.MOVINGOUT].Init(this, _target);
        _states[Plateforms_ENUM.ROTATING].Init(this, _target);
        _states[Plateforms_ENUM.RETURN].Init(this);
    }

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;

        _renderer = GetComponent<MeshRenderer>();

        _tempRendererEnabled = _renderer.enabled;

        if (_target == null)
        {
            Transform target;
            target = transform;
            _target = target;
        }

        _amplitude = Random.Range(_minAmplitude, _maxAmplitude);
        _speed = Random.Range(_minSpeed, _maxSpeed);

        Init();

        _player = PlayerManager.Instance.Player.transform;

        if(PortalManager.Instance.PortalGCF != null && PortalManager.Instance.PortalGCF.PortalID == Portal_ENUM.GCF)
        {
            _portal = null;
            _portal = PortalManager.Instance.PortalGCF.transform;
        }
        else
        {
            _portal = null;
        }

        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    private void OnTick()
    {
        _states[_currentState].Tick();
    }

    private void OnUpdate()
    {
        float DistFromPlayer = Vector3.Distance(_player.position, _startPosition);

        if(_portal != null)
        {
            float DistBetweenPlayerAndPortal = Vector3.Distance(_startPosition, _portal.position);

            if (DistBetweenPlayerAndPortal >= 100)
            {
                if (changeRendererValue == false)
                {
                    _renderer.enabled = false;
                    changeRendererValue = true;
                }
            }
        }
        else
        {
            if (DistFromPlayer >= 100)
            {
                if (changeRendererValue == false)
                {
                    _renderer.enabled = false;
                    changeRendererValue = true;
                }
            }
            else
            {
                if (changeRendererValue == true)
                {
                    if (_currentState == Plateforms_ENUM.STAY || _currentState == Plateforms_ENUM.MOVINGOUT)
                    {
                        _renderer.enabled = _tempRendererEnabled;
                    }
                    else
                    {
                        _renderer.enabled = _shouldRender;
                    }

                    changeRendererValue = false;
                }
                OnTick();
            }
        }
    }

    public void SetState(Plateforms_ENUM nextState)
    {
        _states[_currentState].Exit();
        _states[nextState].Enter();
        _currentState = nextState;
    }

    public void SetOverrideState(bool value)
    {
        if(_isSpecialEvent == true)
        {
            _overrideState = value;
        }
        else
        {
            _overrideState = false;
        }
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
            GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}