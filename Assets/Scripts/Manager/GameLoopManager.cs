using Engine.Singleton;
using System;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{
    #region FIELDS
    private bool _isPaused = false;
    private Vector3 _direction = Vector3.zero;
    public bool IsPaused { get { return _isPaused; }
        set
        {
            _isPaused = value;
            _pause(_isPaused);
        }
    }

    #region EVENT UPDATE
    private event Action _updatePlayer = null;
    public event Action UpdatePlayer
    {
        add
        {
            _updatePlayer -= value;
            _updatePlayer += value;
        }
        remove
        {
            _updatePlayer -= value;
        }
    }

    private event Action<Vector3> _updateGravity = null;
    public event Action<Vector3> UpdateGravity
    {
        add
        {
            _updateGravity -= value;
            _updateGravity += value;
        }
        remove
        {
            _updateGravity -= value;
        }
    }

    private event Action _updateOther = null;
    public event Action UpdateOther
    {
        add
        {
            _updateOther -= value;
            _updateOther += value;
        }
        remove
        {
            _updateOther -= value;
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

    private event Action _updateInput = null;
    public event Action UpdateInput
    {
        add
        {
            _updateInput -= value;
            _updateInput += value;
        }
        remove
        {
            _updateInput -= value;
        }
    }

    private event Action _updateCamera = null;
    public event Action UpdateCamera
    {
        add
        {
            _updateCamera -= value;
            _updateCamera += value;
        }
        remove
        {
            _updateCamera -= value;
        }
    }

    private event Action _updateHeadBobbing = null;
    public event Action UpdateHeadBobbing
    {
        add
        {
            _updateHeadBobbing -= value;
            _updateHeadBobbing += value;
        }
        remove
        {
            _updateHeadBobbing -= value;
        }
    }

    private event Action _updateInteractions = null;
    public event Action UpdateInteractions
    {
        add
        {
            _updateInteractions -= value;
            _updateInteractions += value;
        }
        remove
        {
            _updateInteractions -= value;
        }
    }

    private event Action _updateCanvas = null;
    public event Action UpdateCanvas
    {
        add
        {
            _updateCanvas -= value;
            _updateCanvas += value;
        }
        remove
        {
            _updateCanvas -= value;
        }
    }
    #endregion EVENT UPDATE

    #endregion FIELDS

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

        if(_updateGravity != null)
        {
            _updateGravity(_direction);
        }

        if(_updateCanvas != null)
        {
            _updateCanvas();
        }

        if (_updatePlayer != null)
            {
                _updatePlayer();
            }

        if (_updateCamera != null)
            {
                _updateCamera();
            }

        if (_updateHeadBobbing != null)
            {
                _updateHeadBobbing();
            }

        if (_updateInteractions != null)
            {
                _updateInteractions();
            }

        if(_updateOther != null)
        {
            _updateOther();
        }
    }

    private void FixedUpdate()
    {
        if (_isPaused == false)
        {
            if (_updateInput != null)
            {
                _updateInput();
            }
        }
    }

    protected override void OnDestroy()
    {
        _updatePlayer = null;
        _updateInput = null;
        _updateCamera = null;
        _updateInteractions = null;
        _updateHeadBobbing = null;
        _updateCanvas = null;
        _isPaused = false;
        _updateOther = null;
    }
}
