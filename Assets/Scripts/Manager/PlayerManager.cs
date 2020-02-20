using UnityEngine;
using Engine.Singleton;

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
    #endregion Fields

    #region Methods
    public void InstantiatePlayer(Vector3 position, Quaternion rotation)
    {
        _player = Instantiate(_playerPrefab, position, rotation);
    }
    #endregion Methods
}
