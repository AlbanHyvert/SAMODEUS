using UnityEngine;

public class MovingDeathCube : MonoBehaviour
{
    [SerializeField] private Transform _distanceBeforeDesctruction = null;
    [SerializeField] private float _speed = 3;
    [SerializeField] private float _resizeSpeed = 5;
    
    private float _distanceBeforeNoEffet = 0.4f;
    private float _timer = 0;
    private Vector3 _tempScale = Vector3.zero;
    private float _rescaleTime = 0;
    private Collider _col = null;
    private bool _isTooClose = false;

    public Transform DistanceBeforeDesctruction { get => _distanceBeforeDesctruction; set => _distanceBeforeDesctruction = value; }
    public float Speed { get => _speed; set => _speed = value; }

    private void Start()
    {
        _tempScale = transform.localScale;
        transform.localScale = Vector3.zero;
        GameLoopManager.Instance.Puzzles += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
        _timer = 0;
        _col = transform.GetComponent<Collider>();
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
        
        if(_isTooClose == false)
        {
            _timer = Time.deltaTime;
            _rescaleTime = Time.deltaTime;

            transform.localScale = Vector3.Lerp(transform.localScale, _tempScale, _resizeSpeed * _rescaleTime);
            transform.position = Vector3.Lerp(transform.position, _distanceBeforeDesctruction.position, _speed * _timer);
        }

        if (distBeforeDesctruct < 1f)
        {
            _isTooClose = true;

            if(transform.localScale != Vector3.zero)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, _resizeSpeed * Time.deltaTime);
                float distfromOriginalSize = Vector3.Distance(transform.localScale, _tempScale / 2);

                if (distfromOriginalSize <= _distanceBeforeNoEffet)
                {        
                    _col.enabled = false;
                }
            }
            else
            {
                Object.Destroy(gameObject);
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