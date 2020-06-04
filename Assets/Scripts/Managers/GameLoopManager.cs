using Engine.Singleton;
using System;
using UnityEngine;

public class GameLoopManager : Singleton<GameLoopManager>
{

    private bool _isPaused = false;

    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            SetPause(value);
            _pause(_isPaused);
        }
    }

    #region EVENTS

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

    private event Action _inputManager = null;
    public event Action Inputs
    {
        add
        {
            _inputManager -= value;
            _inputManager += value;
        }
        remove
        {
            _inputManager -= value;
        }
    }

    private event Action _player = null;
    public event Action Player
    {
        add
        {
            _player -= value;
            _player += value;
        }
        remove
        {
            _player -= value;
        }
    }

    private event Action _puzzles = null;
    public event Action Puzzles
    {
        add
        {
            _puzzles -= value;
            _puzzles += value;
        }
        remove
        {
            _puzzles -= value;
        }
    }

    private event Action _camera = null;
    public event Action Camera
    {
        add
        {
            _camera -= value;
            _camera += value;
        }
        remove
        {
            _camera -= value;
        }
    }

    private event Action _managers = null;
    public event Action Managers
    {
        add
        {
            _managers -= value;
            _managers += value;
        }
        remove
        {
            _managers -= value;
        }
    }

    private event Action _ui = null;
    public event Action UI
    {
        add
        {
            _ui -= value;
            _ui += value;
        }
        remove
        {
            _ui -= value;
        }
    }

    #endregion EVENTS

    private void Start()
    {
        _isPaused = false;
    }

    public void SetPause(bool value)
    {
        _isPaused = value;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            IsPaused = !IsPaused;
        }

        if (_managers != null)
        {
            _managers();
        }

        if (_inputManager != null)
        {
            _inputManager();
        }

        if (_player != null)
        {
            _player();
        }

        if(_puzzles != null)
        {
            _puzzles();
        }

        if (_camera != null)
        {
            _camera();
        }

        if (_ui != null)
        {
            _ui();
        }
    }
}