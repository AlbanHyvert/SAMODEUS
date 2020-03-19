using System;
using UnityEngine;
using Engine.Singleton;

public class GameLoopManager : Singleton<GameLoopManager>
{
    [SerializeField] private DataKeycode _dataKeycode = null;

    private bool _isPaused = false;

    public bool IsPaused { get { return _isPaused; }
        set
        {
            _isPaused = value;
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

    #endregion EVENTS

    private void Start()
    {
        _isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(_dataKeycode.KeyPause) || Input.GetKeyDown(_dataKeycode.KeyPauseAlt))
        {
            IsPaused = !IsPaused;
        }

        if(_inputManager != null)
        {
            _inputManager();
        }

        if(_player != null)
        {
            _player();
        }

        if(_camera != null)
        {
            _camera();
        }

        if(_puzzles != null)
        {
            _puzzles();
        }
    }
}
