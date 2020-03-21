using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private GameObject[] _activatedObject = null;

    private void Start()
    {
        if (_activatedObject != null)
        {
            for (int i = 0; i < _activatedObject.Length; i++)
            {
                _activatedObject[i].SetActive(false);
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
        }
    }
}
