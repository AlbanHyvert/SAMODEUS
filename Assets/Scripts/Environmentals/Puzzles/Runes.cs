using UnityEngine;

public class Runes : MonoBehaviour, IInteract
{
    [SerializeField] private int _rotateForce = 30;
    [SerializeField] private int _rotateSpeed = 2;
    private int _matchingValueX = 0;

    private float _valueZ = 0;
    private bool _isMatching = false;

    public bool IsMatching { get { return _isMatching; } }
    public int MatchingValueX { get { return _matchingValueX; } set { _matchingValueX = value; } }

    private void Start()
    {
        Quaternion quaternion = transform.rotation;
        quaternion = new Quaternion(0, 0, 90, 0);

        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    void IInteract.Enter()
    {
        if(_isMatching == false)
        {
            _valueZ += _rotateForce;
        }
    }

    private void OnUpdate()
    {   
        if(_valueZ > 180)
        {
            _valueZ = 0;
        }

        
        if(_valueZ == _matchingValueX)
        {
            _isMatching = true;
        }

        Quaternion targetRot = Quaternion.Euler(transform.rotation.x, transform.rotation.y, _valueZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * _rotateSpeed);
    }

    void IInteract.Exit()
    {

    }


    private void OnDestroy()
    {
        GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}
