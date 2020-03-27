using UnityEngine;

public class Runes : MonoBehaviour, IInteract
{
    private int _positionValue = 1;
    private Vector3 _rotation = Vector3.zero;

    public int PositionValue { get { return _positionValue; } }

    private void Start()
    {
        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    void IInteract.Enter()
    {
        if(_positionValue > 3)
        {
            _positionValue = 1;
        }
        else
        {
            _positionValue++;
        }
    }

    private void OnUpdate()
    {
        if(_positionValue == 1)
        {
            transform.rotation = Quaternion.Euler(0,0, 90);
        }
        else if(_positionValue == 2)
        {
            transform.rotation = Quaternion.Euler(0, 0, 30);
        }
        else if (_positionValue == 3)
        {
            transform.rotation = Quaternion.Euler(0, 0, 10);
        }
        else if (_positionValue == 4)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }

    void IInteract.Exit()
    {

    }


    private void OnDestroy()
    {
        GameLoopManager.Instance.Puzzles -= OnUpdate;
    }
}
