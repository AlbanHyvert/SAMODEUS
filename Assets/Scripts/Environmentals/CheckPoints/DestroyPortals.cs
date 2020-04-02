using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DestroyPortals : MonoBehaviour
{
    [SerializeField] private Portal[] _portals = null;
    [SerializeField] private GameObject[] _plane = null;
    [SerializeField] private GameObject[] _gameObject = null;

    [SerializeField] private float _speedDisolve = 1;
    [SerializeField] private float _maximalTimerValue = 10;

    private bool _isActive = false;
    private float _timer = 0f;

    private void Start()
    {
        GameLoopManager.Instance.Puzzles += OnUpdate;
        _timer = 0.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && _isActive == false)
        {
            _timer = 0.0f;
            _isActive = true;

            if(_portals != null)
            {
                for (int i = 0; i < _portals.Length; i++)
                {
                    Object.Destroy(_portals[i], 1);
                }
            }

            if(_plane != null)
            {
                for (int i = 0; i < _plane.Length; i++)
                {
                    Object.Destroy(_plane[i], 1);
                }
            }
        }
    }

    private void OnUpdate()
    {
        if(_isActive == true)
        {
            _timer += 0.01f * (_speedDisolve* Time.deltaTime);

            if (_gameObject != null)
            {
                for (int i = 0; i < _gameObject.Length; i++)
                {
                    if (_gameObject[i] != null)
                    {
                        Renderer renderer = _gameObject[i].GetComponent<Renderer>();

                        if (renderer != null)
                            renderer.material.SetFloat("Vector1_3996BBE4", _timer);
                    }
                }
            }

            if(_timer >= _maximalTimerValue)
            {
                for (int i = 0; i < _gameObject.Length; i++)
                {
                    Object.Destroy(_gameObject[i]);
                }
            }
        }
        else
        {
            _timer = 0.0f;
        }
    }
}
