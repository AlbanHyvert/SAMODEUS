using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [SerializeField] private float _frequency = 1;
    [SerializeField] private float _amplitude = 1;
    [SerializeField] private float _amplitudeMaxValue = 10;
    private Material _mat = null;
    private float _minRange = 1;
    private float _maxRange = 10;
    private Transform _object = null;
    private Vector3 _startPos = Vector3.zero;
    float _elapsedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _object = this.transform;
        _startPos = _object.position;
        _frequency = Random.Range(0.5f, 2f);
       // _amplitude = Random.Range(0.5f, 2f);
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float timePass = Time.deltaTime * Time.timeScale;

        _elapsedTime += Time.deltaTime * Time.timeScale * _frequency;
        _amplitude = Vector3.Distance(_object.position, PlayerManager.Instance.Player.transform.position);


        if (_amplitude <= 5)
        {
            _amplitude = 0;
            //_amplitude = Mathf.Lerp(_amplitude, 0, timePass);
        }

        transform.position = _startPos + Vector3.up * Mathf.Sin(_elapsedTime) * _amplitude;
    }
}
