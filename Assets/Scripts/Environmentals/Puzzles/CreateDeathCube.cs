using System.Collections.Generic;
using UnityEngine;

public class CreateDeathCube : MonoBehaviour
{
    [SerializeField] private Transform[] _distanceBeforeDesctruction = null;
    [SerializeField] private Transform[] _spawnersPosition = null;
    [SerializeField] private MovingDeathCube[] _deathCube = null;
    [Space]
    [SerializeField] private float _distanceBeforeActivation = 150;
    [Space]
    [SerializeField] private float _timeBeforeSpawn = 3;
    [SerializeField] private float _deathCubeSpeed = 3;

    private float _timer = 0;
    private int _i = 0;
    private List<MovingDeathCube> _deathCubeList = null;

    private void Start()
    {
        _deathCubeList = new List<MovingDeathCube>();
        GameLoopManager.Instance.Puzzles += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
        _timer = Time.time + _timeBeforeSpawn;
    }

    private void IsPaused(bool value)
    {
        if (value == true)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.Puzzles += OnUpdate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if(playerController != null)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
            GameLoopManager.Instance.Pause -= IsPaused;

            for (int i = 0; i < _deathCubeList.Count; i++)
            {
                if(_deathCubeList[i] != null)
                {
                    Object.Destroy(_deathCubeList[i].gameObject);
                }
            }

            _deathCubeList.Clear();
            gameObject.SetActive(false);
        }
    }

    private void OnUpdate()
    {
        float DistFromPlayer = Vector3.Distance(transform.position, PlayerManager.Instance.Player.transform.position);

        if(DistFromPlayer < _distanceBeforeActivation)
        {
            if (Time.time > _timer)
            {
                if (_i < _spawnersPosition.Length)
                {
                    _timer = Time.time + _timeBeforeSpawn;
                    MovingDeathCube deathCube = Instantiate(_deathCube[_i], _spawnersPosition[_i].position, Quaternion.identity);
                    deathCube.DistanceBeforeDesctruction = _distanceBeforeDesctruction[_i];
                    deathCube.Speed = _deathCubeSpeed;
                    deathCube.transform.SetParent(_spawnersPosition[_i]);
                    _i += 1;
                    _deathCubeList.Add(deathCube);
                }
                else
                {
                    _i = 0;
                }
            }
        }
    }

    private void OnDestroy()
    {
        if (GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
            GameLoopManager.Instance.Pause -= IsPaused;
        }
    }
}
