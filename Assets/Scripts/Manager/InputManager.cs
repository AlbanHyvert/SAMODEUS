using Engine.Singleton;
using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private string _IDForward = "FORWARD";
    private string _IDLeft = "LEFT";
    private string _IDBack = "BACK";
    private string _IDRight = "RIGHT";
    private string _IDInteraction = "INTERACTION";

    private Vector3 _direction = Vector3.zero;

    #region KEYCODE
    private KeyCode _keyForward = KeyCode.Z;
    private KeyCode _keyLeft = KeyCode.Q;
    private KeyCode _keyBack = KeyCode.S;
    private KeyCode _keyRight = KeyCode.D;
    private KeyCode _keyInteraction = KeyCode.E;
    private KeyCode _keyThrow = KeyCode.F;
    #endregion KEYCODE

    #region EVENT
    private event Action<Vector3> _movement = null;
    public event Action<Vector3> Move
    {
        add
        {
            _movement -= value;
            _movement += value;
        }
        remove
        {
            _movement -= value;
        }
    }

    private event Action _anyKey = null;
    public event Action AnyKey
    {
        add
        {
            _anyKey -= value;
            _anyKey += value;
        }
        remove
        {
            _anyKey -= value;
        }
    }

    private event Action _idle = null;
    public event Action Idle
    {
        add
        {
            _idle -= value;
            _idle += value;
        }
        remove
        {
            _idle -= value;
        }
    }

    private event Action _pressShift = null;
    public event Action PressShift
    {
        add
        {
            _pressShift -= value;
            _pressShift += value;
        }
        remove
        {
            _pressShift -= value;
        }
    }

    private event Action _releaseShift = null;
    public event Action ReleaseShift
    {
        add
        {
            _releaseShift -= value;
            _releaseShift += value;
        }
        remove
        {
            _releaseShift -= value;
        }
    }

    private event Action<Vector3> _jump = null;
    public event Action<Vector3> Jump
    {
        add
        {
            _jump -= value;
            _jump += value;
        }
        remove
        {
            _jump -= value;
        }
    }

    private event Action _interact = null;
    public event Action Interact
    {
        add
        {
            _interact -= value;
            _interact += value;
        }
        remove
        {
            _interact -= value;
        }
    }

    private event Action _launch = null;
    public event Action Launch
    {
        add
        {
            _launch -= value;
            _launch += value;
        }
        remove
        {
            _launch -= value;
        }
    }
    #endregion EVENT

    #region PROPERTIES
    public Vector3 Direction { get {return _direction; } }
    public KeyCode KeyForward { get => _keyForward; set => _keyForward = value; }
    public KeyCode KeyLeft { get => _keyLeft; set => _keyLeft = value; }
    public KeyCode KeyBack { get => _keyBack; set => _keyBack = value; }
    public KeyCode KeyRight { get => _keyRight; set => _keyRight = value; }
    public KeyCode KeyInteraction { get => _keyInteraction; set => _keyInteraction = value; }
    public KeyCode KeyThrow { get => _keyThrow; set => _keyThrow = value; }
    #endregion PROPERTIES

    #region METHODS
    private void Start()
    {
        GameLoopManager.Instance.GetInput += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.GetInput -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.GetInput += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        if (_anyKey != null)
        {
            _anyKey();
        }

        _direction = Vector3.zero;

        if (Input.GetKey(_keyForward))
        {
            _direction += PlayerManager.Instance.Player.transform.forward;
        }

        if (Input.GetKey(_keyLeft))
        {
            _direction += -PlayerManager.Instance.Player.transform.right;
        }

        if (Input.GetKey(_keyBack))
        {
            _direction += -PlayerManager.Instance.Player.transform.forward;
        }

        if (Input.GetKey(_keyRight))
        {
            _direction += PlayerManager.Instance.Player.transform.right;
        }

        if (_interact != null && Input.GetKeyDown(_keyInteraction))
        {
            _interact();
        }

        if (_pressShift != null && Input.GetKey(KeyCode.LeftShift))
        {
            _pressShift();
        }
        else if(_releaseShift != null)
        {
            _releaseShift();
        }
        /*if (_releaseShift != null && Input.GetKeyUp(KeyCode.LeftShift))
        {
            _releaseShift();
        }*/

        if (_movement != null && _direction != Vector3.zero)
        {
            _movement(_direction);
        }
        else if (_idle != null && _direction == Vector3.zero)
        {
            _idle();
            //_movement(_direction);
        }

        if (_jump != null && Input.GetKeyDown(KeyCode.Space))
        {
            _direction = PlayerManager.Instance.Player.transform.up;
            _jump(_direction);
            _direction = Vector3.zero;
        }

        if (_launch != null && Input.GetMouseButtonDown(0))
        {
            _launch();
        }
    }

    public void ChangeKey(string ID, KeyCode keyCode)
    {
        if (ID == _IDForward)
        {
            _keyForward = keyCode;
            Debug.Log(_keyForward);
        }
        else if (ID == _IDLeft)
        {
            _keyLeft = keyCode;
            Debug.Log(_keyLeft);
        }
        else if (ID == _IDBack)
        {
            _keyBack = keyCode;
            Debug.Log(_keyBack);
        }
        else if (ID == _IDRight)
        {
            _keyRight = keyCode;
            Debug.Log(_keyRight);
        }
        else if (ID == _IDInteraction)
        {
            _keyInteraction = keyCode;
            Debug.Log(_keyInteraction);
        }
    }

    protected override void OnDestroy()
    {
        GameLoopManager.Instance.GetInput -= OnUpdate;
        GameLoopManager.Instance.Pause -= IsPaused;
        _direction = Vector3.zero;
        _movement = null;
        _jump = null;
        _interact = null;
        _launch = null;
        _releaseShift = null;
        _pressShift = null;
    }
    #endregion METHODS
}
