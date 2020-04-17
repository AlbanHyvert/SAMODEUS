﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    private PlayerController _playerController = null;

    public PlayerController PlayerController { get { return _playerController; } }

    private void Awake()
    {
        if(_camera != null)
        {
            _camera.enabled = false;
        }
        PlayerManager.Instance.CreatePlayer(transform.localPosition, transform.rotation);

        _playerController = PlayerManager.Instance.Player;
    }

    private void Update()
    {
        if(_playerController != null && PlayerManager.Instance.Player == null)
        {
            PlayerManager.Instance.AddPlayer(_playerController);
        }
    }
}
