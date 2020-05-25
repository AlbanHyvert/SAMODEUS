using UnityEngine;

public class CandleLight : MonoBehaviour
{
    [SerializeField] private Light[] _lights = null;
    [SerializeField] private float _minRangeValue = 0.1f;
    [SerializeField] private float _maxRangeValue = 0.1f;
    [SerializeField] private float _minIntensityValue = 0.1f;
    [SerializeField] private float _maxIntensityValue = 0.1f;

    private float _time = 0.0f;
    private bool _pingPong = false;

    private void Start()
    {
        GameLoopManager.Instance.Puzzles += OnUpdate;
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void IsPaused (bool value)
    {
        if(value == true)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
        else
            GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    private void OnUpdate()
    {
        if (_time >= 1)
        {
            _pingPong = true;
        }
        else if (_time <= 0)
        {
            _pingPong = false;
        }

        if (_time <= 1 && _pingPong == false)
        {
            _time += 0.1f * Time.deltaTime;
        }
        else if (_time >= 0 && _pingPong == true)
        {
            _time -= 0.1f * Time.deltaTime;
        }


        if (_lights != null)
        {
            for (int i = 0; i < _lights.Length; i++)
            {
                if (_lights[i].intensity < _maxIntensityValue)
                {
                    _lights[i].intensity = Mathf.Lerp(_minIntensityValue, _maxIntensityValue, _time);
                }
                else if(_lights[i].intensity > _minIntensityValue)
                {
                    _lights[i].intensity = Mathf.Lerp(_maxIntensityValue, _minIntensityValue, _time);
                }

                if (_lights[i].range < _maxRangeValue)
                {
                    _lights[i].range = Mathf.Lerp(_minRangeValue, _maxRangeValue, _time);
                }
                else if(_lights[i].range > _minRangeValue)
                {
                    _lights[i].range = Mathf.Lerp(_maxRangeValue, _minRangeValue, _time);
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
