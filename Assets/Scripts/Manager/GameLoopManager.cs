using Engine.Singleton;
using System;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{
    #region Fields
    private bool _isPaused = false;
    private Vector3 _direction = Vector3.zero;
    public bool IsPaused { get { return _isPaused; }
        set
        {
            _isPaused = value;
            _pause(_isPaused);
        }
    }

    private event Action _getPlayer = null;
    public event Action GetPlayer
    {
        add
        {
            _getPlayer -= value;
            _getPlayer += value;
        }
        remove
        {
            _getPlayer -= value;
        }
    }

    private event Action<Vector3> _setGravity = null;
    public event Action<Vector3> SetGravity
    {
        add
        {
            _setGravity -= value;
            _setGravity += value;
        }
        remove
        {
            _setGravity -= value;
        }
    }

    private event Action _getOther = null;
    public event Action GetOther
    {
        add
        {
            _getOther -= value;
            _getOther += value;
        }
        remove
        {
            _getOther -= value;
        }
    }

    private event Action<bool> _pause = null;
    public event Action<bool> Pause
    {
        add
        {
            _pause -= value;
            _pause += value;
        }
        remove
        {
            _pause -= value;
        }
    }

    private event Action _getInput = null;
    public event Action GetInput
    {
        add
        {
            _getInput -= value;
            _getInput += value;
        }
        remove
        {
            _getInput -= value;
        }
    }

    private event Action _getCamera = null;
    public event Action GetCamera
    {
        add
        {
            _getCamera -= value;
            _getCamera += value;
        }
        remove
        {
            _getCamera -= value;
        }
    }

    private event Action _getHeadBobbing = null;
    public event Action GetHeadBobbing
    {
        add
        {
            _getHeadBobbing -= value;
            _getHeadBobbing += value;
        }
        remove
        {
            _getHeadBobbing -= value;
        }
    }

    private event Action _getInteractions = null;
    public event Action GetInteractions
    {
        add
        {
            _getInteractions -= value;
            _getInteractions += value;
        }
        remove
        {
            _getInteractions -= value;
        }
    }

    private event Action _getCanvas = null;
    public event Action GetCanvas
    {
        add
        {
            _getCanvas -= value;
            _getCanvas += value;
        }
        remove
        {
            _getCanvas -= value;
        }
    }
    #endregion Fields

    private void Start()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if (_pause != null && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            IsPaused = !IsPaused;
        }

        if(_setGravity != null)
        {
            _setGravity(_direction);
        }
        if(_getCanvas != null)
        {
            _getCanvas();
        }

        if (_getPlayer != null)
            {
                _getPlayer();
            }

        if (_getCamera != null)
            {
                _getCamera();
            }

        if (_getHeadBobbing != null)
            {
                _getHeadBobbing();
            }

        if (_getInteractions != null)
            {
                _getInteractions();
            }

        if(_getOther != null)
        {
            _getOther();
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            if (_getInput != null)
            {
                _getInput();
            }
        }
    }

    protected override void OnDestroy()
    {
        _getPlayer = null;
        _getInput = null;
        _getCamera = null;
        _getInteractions = null;
        _getHeadBobbing = null;
        _getCanvas = null;
        _isPaused = false;
        _getOther = null;
    }
}
