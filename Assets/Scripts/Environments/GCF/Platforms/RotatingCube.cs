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
    [SerializeField] private int _maxAmplitude = 30;
    [SerializeField] private int _minAmplitude = 10;
    [Header("StayIn Amplitude")]
    [SerializeField] private float _stayAmplitude = 1.5f;
    [Space]
    [Header("Max and Min Speed")]
    [SerializeField] private float _maxSpeed = 30;
    [SerializeField] private float _minSpeed = 10;
    [Header("Return Speed")]
    [SerializeField] private float _returnSpeed = 20;
    [Header("Stay In Place Speed")]
    [SerializeField] private float _staySpeed = 2;
    [Space]
    [Header("Is A Special Event")]
    [SerializeField] private bool _isSpecialEvent = false;
    #endregion SERIALIZEFIELD

    private bool _overrideState = false;
    private int _amplitude = 0;
    private float _speed = 0;
    private Dictionary<Plateforms_ENUM, IPlateforms> _states = null;
    private Transform _player = null;
    private Vector3 _startPosition = Vector3.zero;
    private Quaternion _startRotation = Quaternion.identity;

    #region PROPERTIES
    public bool IsSpecialEvent { get { return _isSpecialEvent; } }
    public bool OverrideState { get { return _overrideState; } set { SetOverrideState(value); } }
    public Material StayMaterial { get { return _stayMaterial; } }
    public Material MovingOutMaterial { get { return _movingOutMaterial; } }
    public Material ReturnMaterial { get { return _returnMaterial; } }
    public Material RotatingMaterial { get { return _rotatingMaterial; } }
    public Transform Player { get { return _player; } }
    public float MaxDistFromPlayer { get { return _maxDistanceFromPlayer; } }
    public bool ShouldRender { get { return _shouldRender; } set { _shouldRender = value; } }
    public int Amplitude { get { return _amplitude; } }
    public float StayAmplitude { get { return _stayAmplitude; } }
    public float Speed { get { return _speed; } }
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
        if(_shouldRender == false)
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            renderer.enabled = false;
        }

        _startPosition = transform.position;
        _startRotation = transform.rotation;

        if(_target == null)
        {
            Transform target;
            target = transform;
            _target = target;
        }

        _amplitude = Random.Range(_minAmplitude, _maxAmplitude);
        _speed = Random.Range(_minSpeed, _maxSpeed);

        Init();

        _player = PlayerManager.Instance.Player.transform;

        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    private void OnUpdate()
    {
        float DistFromPlayer = Vector3.Distance(_player.position, _startPosition);
        MeshRenderer renderer = GetComponent<MeshRenderer>();

        if(DistFromPlayer <= _maxDistanceFromPlayer)
        {
            renderer.enabled = true;
        }
        else
        {
            if(renderer.enabled != _shouldRender)
            {
                renderer.enabled = _shouldRender;
            }
        }

        _states[_currentState].Tick();
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
}