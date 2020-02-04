using UnityEngine;

public class TimeSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _spawnee = null;
    [SerializeField] private PlayerController _playerController = null;
    private bool _stopSpawning = true;
    public bool StopSpawning { get { return _stopSpawning; } set { _stopSpawning = value; } }
    [SerializeField] private float _spawnDelay = 0;
    [SerializeField] private float _spawnRepeat = 0;
    [SerializeField] private int _numberOfRepeat = 0;
    private int tempRepeat = 0;

    public void DoSpawn(bool value)
    {
        tempRepeat = _numberOfRepeat;
        _stopSpawning = value;

        if(_stopSpawning == false)
        {
            _numberOfRepeat--;
            InvokeRepeating("SpawnObject", _spawnDelay, _spawnRepeat);
        }
    }

    private void SpawnObject()
    {
        _numberOfRepeat--;
        Debug.Log(_numberOfRepeat);

        if (_numberOfRepeat <= 0)
        {
            _stopSpawning = true;
            _numberOfRepeat = tempRepeat;

        }

        if(_stopSpawning == true)
        {
            CancelInvoke("SpawnObject");
        }

        /*if(_playerController.IsGrounded == true)
        {
            Instantiate(_spawnee, transform.position, transform.rotation);
        }*/
    }
}
