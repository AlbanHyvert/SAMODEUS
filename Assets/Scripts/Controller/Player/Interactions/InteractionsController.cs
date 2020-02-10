using UnityEngine;

public class InteractionsController : MonoBehaviour
{
    [SerializeField, Header("Layers For Interactable")] private LayerMask _layerMask = 0;
    [SerializeField, Header("Player Controller")] private PlayerController _playerController = null;
    [SerializeField, Header("Throw Force")] private float _throwForce = 100;

    private Camera _playerCam = null;
    private Runes _runes = null;
    private Rigidbody _playerRb = null;
    private Pickable _object = null;
    private ButtonsInteract _interactable = null;
    private Transform _pickable = null;

    private bool _isPick = false;
    private bool _canPick = false;
    private bool _canInteract = false;

    public Rigidbody PlayerRb { get { return _playerRb; } set { _playerRb = value; } }
    public Pickable Object { get { return _object; } set { _object = value; } }
    public ButtonsInteract Interact { get { return _interactable; } set { _interactable = value; } }
    public Transform Pickable { get { return _pickable; } }
    public bool IsPick { get { return _isPick; } set { _isPick = value; } }

    private void Start()
    {
        _playerCam = _playerController.PlayerCamera;
        InputManager.Instance.Interact += PickUp;
        InputManager.Instance.Interact += OnInteract;
    }

    public void PickUp()
    {
        if(_canPick == true)
        {
            _object.ActionEnter(_playerCam.transform);
            _object.GetPlayer(this);
            _pickable = _object.transform;
            InputManager.Instance.Interact += Drop;
            InputManager.Instance.Launch += Throw;
            InputManager.Instance.Interact -= PickUp;
            _isPick = true;
        }
    }

    public void Drop()
    {
        if(_isPick == true)
        {
            _object.GetPlayer(null);
            _object.ActionExit();
            _object.CustomHighlightExit();
            InputManager.Instance.Interact -= Drop;
            InputManager.Instance.Interact += PickUp;
            InputManager.Instance.Launch -= Throw;
            _isPick = false;
        }
    }

    private void Throw()
    {
        if(_isPick == true)
        {
            _object.ActionExit();
            _object.GetPlayer(null);
            Rigidbody obj = _object.GetComponent<Rigidbody>();
            obj.AddForceAtPosition(_playerCam.transform.forward * _throwForce, _object.transform.position, ForceMode.Impulse);
            InputManager.Instance.Launch -= Throw;
            InputManager.Instance.Interact += PickUp;
            _isPick = false;
        }
    }

    public void IsPaused(bool pause)
    {
        if (pause == true)
        {
            GameLoopManager.Instance.UpdateInteractions -= OnUpdate;
        }
        else
        {
            GameLoopManager.Instance.UpdateInteractions += OnUpdate;
        }
    }

    public void OnUpdate()
    {
        RaycastHit hit;
        bool interactibleCheck = Physics.Raycast(_playerCam.transform.position, _playerCam.transform.forward, out hit, 3, _layerMask);

        if (interactibleCheck == true)
        {
            _object = hit.transform.GetComponent<Pickable>();
            _interactable = hit.transform.GetComponent<ButtonsInteract>();
            _runes = hit.transform.GetComponent<Runes>();

            if (_object != null)
            {
                _canPick = true;
                _object.CustomHighlightEnter();
            }
            else if (_interactable != null)
            {
                _canInteract = true;
                ICustomHighlight customHighlight = _interactable.GetComponent<ICustomHighlight>();
                if (customHighlight != null)
                {
                    customHighlight.Enter();
                }
            }
            else if(_runes != null)
            {
                _canInteract = true;
            }
        }
        else
        {
            _canPick = false;
            _canInteract = false;
            _interactable = null;
            _runes = null;

            if (_object != null)
                _object.CustomHighlightExit();
        }
    }

    private void OnInteract()
    {
        if(_canInteract == true)
        {
            if(_interactable != null)
            {
                IAction action = _interactable.GetComponent<IAction>();
                action.Enter();
            }
            else if(_runes != null)
            {
                IAction action = _runes.GetComponent<IAction>();
                action.Enter();
            }
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.Interact -= PickUp;
        InputManager.Instance.Interact -= OnInteract;
        GameLoopManager.Instance.UpdateInteractions -= OnUpdate;
        InputManager.Instance.Interact -= Drop;
        InputManager.Instance.Interact -= Throw;
    }
}
