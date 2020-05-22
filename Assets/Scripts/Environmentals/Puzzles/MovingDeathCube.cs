using UnityEngine;

public class MovingDeathCube : MonoBehaviour
{
    [SerializeField] private Transform _distanceBeforeDesctruction;
    [SerializeField] private float _speed = 3;

    private float _timer = 0;

    public Transform DistanceBeforeDesctruction { get => _distanceBeforeDesctruction; set => _distanceBeforeDesctruction = value; }
    public float Speed { get => _speed; set => _speed = value; }

    private void Start()
    {
        GameLoopManager.Instance.Puzzles += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void IsPaused(bool value)
    {
        if(value == true)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.Puzzles += OnUpdate;
        }
    }

    private void OnUpdate()
    {
        float distBeforeDesctruct = Vector3.Distance(transform.position, _distanceBeforeDesctruction.position);

        _timer = _speed * Time.deltaTime;

        if ( distBeforeDesctruct > 2)
        {
            transform.position = Vector3.Lerp(transform.position, _distanceBeforeDesctruction.position, _timer * Time.deltaTime);
        }
        else
        {
            Object.Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.Puzzles -= OnUpdate;
        GameLoopManager.Instance.Pause -= IsPaused;
    }
}
