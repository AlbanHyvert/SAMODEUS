using Engine.Singleton;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private PlayerController _playerController = null;

    private PlayerController _player = null;

    public PlayerController Player { get { return _player; } }

    public void CreatePlayer(Vector3 pos, Quaternion rot)
    {
        _player = Instantiate(_playerController, pos, rot);
    }

    public void DestroyPlayer()
    {
        Object.DestroyImmediate(_player);
        _player = null;
    }
}
