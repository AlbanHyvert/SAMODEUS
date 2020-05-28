using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private RotatingCube[] _activatedBridge = null;
    [SerializeField] private Collider[] _colliders = null;

    private void Start()
    {
        if (_colliders != null)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IAction action = other.GetComponent<IAction>();

        if (action != null && other.tag == _objectTag)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerController>().OnDrop();
            action.DestroySelf(_setPosition);

            if(_colliders != null)
            {
                for (int i = 0; i < _colliders.Length; i++)
                {
                    _colliders[i].enabled = true;
                }
            }

            if (_activatedBridge != null)
            {
                for (int i = 0; i < _activatedBridge.Length; i++)
                {
                    _activatedBridge[i].SetOverrideState(true);
                }
            }
        }
    }
}
