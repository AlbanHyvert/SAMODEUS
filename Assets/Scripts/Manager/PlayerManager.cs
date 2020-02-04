using UnityEngine;
using Engine.Singleton;
using System;
using System.Collections.Generic;

public class PlayerManager : Singleton<PlayerManager>
{
    #region Enum
    public enum PlayerState
    {
        ALIVE,
        DEAD
    }
    #endregion Enum

    #region Fields
    [SerializeField ,Header("Player Controller")] private PlayerController _playerPrefab = null;

    private PlayerController _player = null;
    public PlayerController Player { get { return _player; } }

    private PlayerState _currentState = PlayerState.ALIVE;
    private Dictionary<PlayerState, IPlayerState> _states = null;

    #region Properties
    public PlayerState CurrentState { get { return _currentState; } }
    public IPlayerState CurrentStateType { get { return _states[_currentState]; } }
    #endregion Properties

    #endregion Fields

    #region Methods
    public void InstantiatePlayer(Vector3 position, Quaternion rotation)
    {
        _player = Instantiate(_playerPrefab, position, rotation);
    }

    private void Start()
    {
        if (_playerPrefab == null)
        {
            throw new System.Exception("PlayerManager is trying to access a missing component." + _playerPrefab + "Exiting.");
        }
    }
    #endregion Methods
}
