using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Scale : MonoBehaviour
{
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private GameObject[] _activatedObject = null;
    [SerializeField] private GameObject[] _desactivedObject = null;

    private void Start()
    {
        if (_activatedObject != null)
        {
            for (int i = 0; i < _activatedObject.Length; i++)
            {
                _activatedObject[i].SetActive(false);
            }
        }

        if (_desactivedObject != null)
        {
            for (int i = 0; i < _desactivedObject.Length; i++)
            {
                _desactivedObject[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IAction action = other.GetComponent<IAction>();

        if(action != null && other.tag == _objectTag)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerController>().OnDrop();
            action.DestroySelf(_setPosition);
            
            if(_activatedObject != null)
            {
                for (int i = 0; i < _activatedObject.Length; i++)
                {
                    _activatedObject[i].SetActive(true);
                }
            }

            if (_desactivedObject != null)
            {
                for (int i = 0; i < _desactivedObject.Length; i++)
                {
                    _desactivedObject[i].SetActive(false);
                }
            }
        }
    }
}
