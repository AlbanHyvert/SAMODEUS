using Engine.Singleton;
using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Vector3 _direction = Vector3.zero;
    private int _speed = 2;

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
        _direction = Vector3.zero;
        if (Input.GetKey(InputFieldManager.Instance.ForwardMovement))
        {
            _direction += PlayerManager.Instance.Player.transform.forward;
        }

        if (Input.GetKey(InputFieldManager.Instance.LeftMovement))
        {
            _direction += -PlayerManager.Instance.Player.transform.right;
        }

        if (Input.GetKey(InputFieldManager.Instance.BackwardMovement))
        {
            _direction += -PlayerManager.Instance.Player.transform.forward;
        }

        if (Input.GetKey(InputFieldManager.Instance.RightMovement))
        {
            _direction += PlayerManager.Instance.Player.transform.right;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && _pressShift != null)
        {
            _pressShift();
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && _releaseShift != null)
        {
            _releaseShift();
        }
    
        if(_movement != null && _direction != Vector3.zero)
        {
            _movement(_direction);
        }
        else if(_idle != null && _direction == Vector3.zero)
        {
            _idle();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jump != null)
        {
            _direction = PlayerManager.Instance.Player.transform.up;
            _jump(_direction);
            _direction = Vector3.zero;
        }

        if(Input.GetKeyDown(InputFieldManager.Instance.Interactions.Normalize()) && _interact != null)
        {
            _interact();
        }

        if(Input.GetMouseButtonDown(0) && _launch != null)
        {
            _launch();
        }
    }

    protected override void OnDestroy()
    {
        GameLoopManager.Instance.GetInput -= OnUpdate;
        GameLoopManager.Instance.Pause  -= IsPaused;
        _direction = Vector3.zero;
        _movement = null;
        _jump = null;
        _interact = null;
        _launch = null;
        _releaseShift = null;
        _pressShift = null;
    }
}
