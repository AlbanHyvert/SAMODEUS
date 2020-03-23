using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roomsPrefab = null;
    [SerializeField] private float _roomsLenth = 0;
    [SerializeField] private int _safeZone = 10;
    [SerializeField] private int _amountRoomsOnScreen = 7;
    
    private List<GameObject> _roomsList = null;
    private Transform _player = null;
    private float _spawnZ = 0.0f;

    private void Start()
    {
        _roomsList = new List<GameObject>();
        _player = PlayerManager.Instance.Player.transform;
        GameLoopManager.Instance.Puzzles += OnUpdate;

        for (int i = 0; i < _amountRoomsOnScreen; i++)
        {
            CreateRooms(Random.Range(0, _roomsPrefab.Length));
        }
    }

    private void OnUpdate()
    {
        if(_player.position.z - _safeZone > (_spawnZ - _amountRoomsOnScreen * _roomsLenth))
        {
            CreateRooms(Random.Range(0,_roomsPrefab.Length));
            DeleteRooms();
        }
    }
    
    private void CreateRooms(int prefabIndex)
    {
       GameObject go = Instantiate(_roomsPrefab[prefabIndex], transform.position, transform.rotation);
        go.transform.position += transform.right * _spawnZ;
        go.transform.SetParent(transform);
        _roomsList.Add(go);
        _spawnZ += _roomsLenth;
    }

    private void DeleteRooms()
    {
        Object.Destroy(_roomsList[0]);
        _roomsList.RemoveAt(0);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}
