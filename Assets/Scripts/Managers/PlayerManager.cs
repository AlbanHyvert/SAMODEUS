using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerController _playerPrefab = null;

    private PlayerController _player = null;
    private bool _playerCanMoveCamera = true;
    private bool _playerCanMove = true;
    private bool _useGravity = true;
    [Range(0,100)] private float _musicVolume = 30f;
    [Range(0,100)] private float _dialsVolume = 70f;

    private event Action<bool> _cameraCanMove = null;
    public event Action<bool> CameraCanMove
    {
        add
        {
            _cameraCanMove -= value;
            _cameraCanMove += value;
        }
        remove
        {
            _cameraCanMove -= value;
        }
    }

    private event Action<bool> _shouldMove = null;
    public event Action<bool> ShouldMove
    {
        add
        {
            _shouldMove -= value;
            _shouldMove += value;
        }
        remove
        {
            _shouldMove -= value;
        }
    }

    private event Action<bool> _haveGravity = null;
    public event Action<bool> HaveGravity
    {
        add
        {
            _haveGravity -= value;
            _haveGravity += value;
        }
        remove
        {
            _haveGravity -= value;
        }
    }

    private event Action<WorldEnum> _worldMusic = null;
    public event Action<WorldEnum> WorldMusic
    {
        add
        {
            _worldMusic -= value;
            _worldMusic += value;
        }
        remove
        {
            _worldMusic -= value;
        }
    }

    public PlayerController PlayerPrefab { get { return _playerPrefab; } }
    public PlayerController Player { get { return _player; } set { SetPlayer(value); } }
    public bool PlayerCanMoveCamera { get { return _playerCanMoveCamera; }
        set
        {
            SetPlayerCanMoveCamera(value);
            _cameraCanMove(_playerCanMoveCamera);
        }
    }
    public bool PlayerCanMove { get { return _playerCanMove; }
        set
        {
            SetCanPlayerMove(value);
            _shouldMove(_playerCanMove);
        }
    }
    public bool UseGravity { get { return _useGravity; } 
        set
        {
            SetUseGravity(value);
            _haveGravity(_useGravity);
        } 
    }
    public float MusicVolume { get { return _musicVolume; } set { _musicVolume = value; } }
    public float DialVolume { get { return _dialsVolume; } set { _dialsVolume = value; } }

    public void SetCurrentWorldMusic(WorldEnum world)
    {
        if(_worldMusic != null)
        {
            _worldMusic(world);
        }
    }

    private void SetPlayerCanMoveCamera(bool value)
    {
        _playerCanMoveCamera = value;
    }

    private void SetUseGravity(bool value)
    {
        _useGravity = value;
    }

    private void SetCanPlayerMove(bool value)
    {
        _playerCanMove = value;
    }

    private void SetPlayer(PlayerController player)
    {
        _player = player;
    }
}