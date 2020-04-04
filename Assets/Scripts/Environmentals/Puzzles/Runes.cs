using UnityEngine;

public class Runes : MonoBehaviour, IInteract
{
    [Range(1,4)] private int _positionValue = 1;
    private Vector3 _rotation = Vector3.zero;

    public int PositionValue { get { return _positionValue; } }

    private void Start()
    {
        GameLoopManager.Instance.Puzzles += OnUpdate;

        int rdmRot = Random.Range(1, 3);

        if(rdmRot > 1 && rdmRot < 4)
        {
            _positionValue = rdmRot;
        }
        else
        {
            _positionValue = 2;
        }
    }

    void IInteract.Enter()
    {
        _positionValue++;
    }

    private void OnUpdate()
    {
        if (_positionValue > 4)
        {
            _positionValue = 1;
        }

        if (_positionValue == 1)
        {
            transform.rotation = Quaternion.Euler(10, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if(_positionValue == 2)
        {
            transform.rotation = Quaternion.Euler(45, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if (_positionValue == 3)
        {
            transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else if (_positionValue == 4)
        {
            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
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
