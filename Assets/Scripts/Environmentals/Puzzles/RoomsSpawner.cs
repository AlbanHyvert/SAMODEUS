using System.Collections.Generic;
using UnityEngine;

public class RoomsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _roomsPrefab = null;
    [SerializeField] private float _roomsLenth = 0;
    [SerializeField] private int _distanceBeforeSpawning = 10;
    [SerializeField] private int _amountRoomsOnScreen = 7;

    private float _distance = 100;
    private float _distanceFirstRoom = 100;
    private List<GameObject> _roomsList = null;
    private Transform _player = null;
    private float _spawnZ = 0.0f;

    private void Start()
    {
        InfiniteRoomManager.Instance.RoomsSpawner = this;

        _roomsList = new List<GameObject>();
        _player = PlayerManager.Instance.Player.transform;
        GameLoopManager.Instance.Puzzles += OnUpdate;

        for (int i = 0; i < _amountRoomsOnScreen; i++)
        {
            CreateRoomsForward(Random.Range(0, _roomsPrefab.Length));
        }
    }

    private void OnUpdate()
    {
         _distance = Vector3.Distance(_player.position, _roomsList[_roomsList.Count -1].transform.position);
        _distanceFirstRoom = Vector3.Distance(_player.position, _roomsList[1].transform.position);

         if (_distance < _distanceBeforeSpawning)
         {
             CreateRoomsForward(Random.Range(0, _roomsPrefab.Length));
             DeleteRooms();
         }
    }
    
    private void CreateRoomsForward(int prefabIndex)
    {
       GameObject go = Instantiate(_roomsPrefab[prefabIndex], transform.position, transform.rotation);
        go.transform.SetParent(transform);
        go.transform.position += transform.right * _spawnZ;
        _roomsList.Add(go);

        _spawnZ += _roomsLenth;
    }

    private void DeleteRooms()
    {
        Destroy(_roomsList[0]);
        _roomsList.RemoveAt(0);
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
            GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}
