using System;
using UnityEngine;
using Engine.Singleton;

public class InputManager : Singleton<InputManager>
{
    public enum Keys
    {
        FORWARD,
        LEFT,
        BACK,
        RIGHT,
        INTERACTION,
        SPRINT,
        DIALOGUE
    }

    [SerializeField] private DataKeycode _dataKeyCode = null;
    private int _verticalSensitivity = 10; 
    private int _horizontalSensitivity = 10;

    private Vector3 _direction = Vector3.zero;

    public Vector3 Direction { get { return _direction; } }
    public int VerticalSensitivity { get { return _verticalSensitivity; } set { _verticalSensitivity = value; } }
    public int HorizontalSensitivity { get { return _horizontalSensitivity; } set { _horizontalSensitivity = value; } }
    public  DataKeycode DataKeycode { get { return _dataKeyCode; } }

    #region EVENTS
    private event Action<Vector3> _movement = null;
    public event Action<Vector3> Movement
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

    private event Action _pressSprint = null;
    public event Action PressSprint
    {
        add
        {
            _pressSprint -= value;
            _pressSprint += value;
        }
        remove
        {
            _pressSprint -= value;
        }
    }

    private event Action _releaseSprint = null;
    public event Action ReleaseSprint
    {
        add
        {
            _releaseSprint -= value;
            _releaseSprint += value;
        }
        remove
        {
            _releaseSprint -= value;
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

    private event Action<Vector3> _gravity = null;
    public event Action<Vector3> Gravity
    {
        add
        {
            _gravity -= value;
            _gravity += value;
        }
        remove
        {
            _gravity -= value;
        }
    }

    private event Action _interaction = null;
    public event Action Interaction
    {
        add
        {
            _interaction -= value;
            _interaction += value;
        }
        remove
        {
            _interaction -= value;
        }
    }

    private event Action _throw = null;
    public event Action Throw
    {
        add
        {
            _throw -= value;
            _throw += value;
        }
        remove
        {
            _throw -= value;
        }
    }

    private event Action _passDialogue = null;
    public event Action PassDialogue
    {
        add
        {
            _passDialogue -= value;
            _passDialogue += value;
        }
        remove
        {
            _passDialogue -= value;
        }
    }

    private event Action _changeInput = null;
    public event Action ChangeInput
    {
        add
        {
            _changeInput -= value;
            _changeInput += value;
        }
        remove
        {
            _changeInput -= value;
        }
    }
    #endregion EVENTS

    public void ChangeKey(Keys keys, KeyCode keyCode)
    {
        if(keys == Keys.FORWARD)
        {
            _dataKeyCode.KeyForward = keyCode;
        }

        else if (keys == Keys.LEFT)
        {
            _dataKeyCode.KeyLeft = keyCode;
        }

        else if (keys == Keys.BACK)
        {
            _dataKeyCode.KeyBack = keyCode;
        }

        else if (keys == Keys.RIGHT)
        {
            _dataKeyCode.KeyRight = keyCode;
        }

        else if(keys == Keys.INTERACTION)
        {
            _dataKeyCode.KeyInteraction = keyCode;
        }
        else if(keys == Keys.SPRINT)
        {
            _dataKeyCode.KeySprint = keyCode;
        }
        else if (keys == Keys.DIALOGUE)
        {
            _dataKeyCode.KeyDialogue = keyCode;
        }
    }

    private void Start()
    {
        GameLoopManager.Instance.Inputs += OnUpdate;
        GameLoopManager.Instance.Pause += IsPause;
    }

    private void IsPause(bool pause)
    {
        if(pause == true)
        {
            GameLoopManager.Instance.Inputs -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.Inputs += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        _direction = Vector3.zero;

        if(PlayerManager.Instance.Player != null)
        {
            if (Input.GetKey(_dataKeyCode.KeyForward))
            {
                _direction += PlayerManager.Instance.Player.transform.forward;
            }

            if (Input.GetKey(_dataKeyCode.KeyBack))
            {
                _direction += -PlayerManager.Instance.Player.transform.forward;
            }

            if (Input.GetKey(_dataKeyCode.KeyLeft))
            {
                _direction += -PlayerManager.Instance.Player.transform.right;
            }

            if (Input.GetKey(_dataKeyCode.KeyRight))
            {
                _direction += PlayerManager.Instance.Player.transform.right;
            }
        }

        if(_interaction != null && Input.GetKeyDown(_dataKeyCode.KeyInteraction))
        {
            _interaction();
        }

        if (_pressSprint != null && Input.GetKeyDown(_dataKeyCode.KeySprint))
        {
            _pressSprint();
        }

        if (_releaseSprint != null && Input.GetKeyUp(_dataKeyCode.KeySprint))
        {
            _releaseSprint();
        }

        if(_passDialogue != null && Input.GetKeyDown(_dataKeyCode.KeyDialogue))
        {
            _passDialogue();
        }

        if(_throw != null && Input.GetMouseButtonDown(0))
        {
            _throw();
        }

        if(_movement != null && _direction != Vector3.zero)
        {
            _movement(_direction.normalized);
        }

        if(_idle != null && _direction == Vector3.zero)
        {
            _idle();
        }

        if(_gravity != null)
        {
            _gravity(_direction.normalized);
        }

        if (_changeInput != null)
        {
            _changeInput();
        }
    }
}
