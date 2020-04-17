using Engine.Singleton;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public enum WorldTag
    {
        VERTUMNE,
        GCF,
        DEV
    }

    [SerializeField] private PlayerController _playerController = null;

    private float _musicVolume = 0.0f;
    private float _dialsVolume = 0.0f;
    private PlayerController _player = null;

    public PlayerController Player { get { return _player; } }
    public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
    public float DialsVolume { get => _dialsVolume; set => _dialsVolume = value; }

    public void CreatePlayer(Vector3 pos, Quaternion rot)
    {
        _player = Instantiate(_playerController, pos, rot);
        _player.MusicAudioSource.volume = _musicVolume;
        _player.DialsAudioSource.volume = _dialsVolume;
    }

    public void AddPlayer(PlayerController playerController)
    {
        _player = playerController;
    }

    public void DestroyPlayer()
    {
        if(_player != null)
        {
            Object.Destroy(_player);
            _player = null;
        }
    }
}
