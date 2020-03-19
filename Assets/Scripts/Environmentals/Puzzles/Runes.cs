using UnityEngine;

public class Runes : MonoBehaviour, IInteract
{
    [SerializeField] private int _rotateForce = 30;
    [SerializeField] private int _rotateSpeed = 2;
    private int _matchingValueX = 0;

    private float _valueX = 0;
    private bool _isMatching = false;

    public bool IsMatching { get { return _isMatching; } }
    public int MatchingValueX { get { return _matchingValueX; } set { _matchingValueX = value; } }

    private void Start()
    {
        _valueX = transform.eulerAngles.z;
        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    void IInteract.Enter()
    {
        if(_isMatching == false)
        {
            _valueX += _rotateForce;
        }
    }

    private void OnUpdate()
    {
        if(_valueX == _matchingValueX)
        {
            _isMatching = true;
        }

        Quaternion targetRot = Quaternion.Euler(_valueX, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * _rotateSpeed);
    }

    void IInteract.Exit()
    {

    }
}
