using Engine.Singleton;
using System;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerController _playerPrefab = null;

    private PlayerController _player = null;
    private bool _playerCanMove = true;
    private bool _useGravity = true;
    private float _musicVolume = 1;
    private float _dialsVolume = 1;

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

    public PlayerController PlayerPrefab { get { return _playerPrefab; } }
    public PlayerController Player { get { return _player; } set { SetPlayer(value); } }
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