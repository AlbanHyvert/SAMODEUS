using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roomsPrefab = null;
    [SerializeField] private float _roomsLenth = 0;
    [SerializeField] int _amountRoomsOnScreen = 7;

    private Transform _player = null;
    private float _spawnZ = 0.0f;

    private void Start()
    {
        _player = PlayerManager.Instance.Player.transform;
        GameLoopManager.Instance.Puzzles += OnUpdate;
        for (int i = 0; i < _amountRoomsOnScreen; i++)
        {
            CreateRooms(Random.Range(0, _roomsPrefab.Length));
        }
    }

    private void OnUpdate()
    {
        if(_player.position.x > (_spawnZ - _amountRoomsOnScreen * _roomsLenth))
        {
            CreateRooms(Random.Range(0,_roomsPrefab.Length));
        }
    }

    private void CreateRooms(int prefabIndex)
    {
       GameObject go = Instantiate(_roomsPrefab[prefabIndex], transform.position, transform.rotation);
        go.transform.position += transform.right * _spawnZ;
        _spawnZ += _roomsLenth;
    }
}
