using UnityEngine;
using SAMODEUS.Movements;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private DataMovements _dataMovements = null;
    [SerializeField] private Camera _camera = null;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private LayerMask _activeLayer = 0;
    [SerializeField] private int _distanceInteract = 5;
    [SerializeField] private int _throwForce = 20;

    private CharacterController _controller = null;
    private bool _hit = false;
    private GameObject _interactableObj = null;

    [SerializeField] private PlayerManager.WorldTag _worldTag = PlayerManager.WorldTag.VERTUMNE;

    public Camera Camera { get { return _camera; } }
    public AudioSource AudioSource { get { return _audioSource; } }

    public PlayerManager.WorldTag WorldTaged { get { return _worldTag; } set { _worldTag = value; } }
    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        if(_camera == null)
        {
            _camera = GetComponentInChildren<Camera>();

            if (_camera == null)
            {
                Debug.LogError("Missing Component : " + typeof(Camera));
            }
        }

        GameLoopManager.Instance.Player += OnRaycast;
        GameLoopManager.Instance.Pause += IsPaused;
        InputManager.Instance.Movement += OnMovements;
        InputManager.Instance.Idle += OnIdle;
        InputManager.Instance.Interaction += OnPickUp;
        InputManager.Instance.Interaction += OnInteract;
        InputManager.Instance.PressSprint += OnSprint;
        InputManager.Instance.ReleaseSprint += OnWalk;
    }

    private void IsPaused(bool pause)
    {
        if(pause == true)
        {
            GameLoopManager.Instance.Player -= OnRaycast;
            InputManager.Instance.Movement -= OnMovements;
            InputManager.Instance.Idle -= OnIdle;
        }
        else
        {
            GameLoopManager.Instance.Player += OnRaycast;
            InputManager.Instance.Movement += OnMovements;
            InputManager.Instance.Idle += OnIdle;
        }
    }

    private void OnIdle()
    {
        OnGravity(Vector3.zero);
    }

    private void OnMovements(Vector3 dir)
    {
        dir *= _dataMovements.MoveSpeed * _dataMovements.SprintMult;
        OnGravity(dir);
        _controller.Move(dir * Time.deltaTime);
    }

    private void OnSprint()
    {
        _dataMovements.SprintMult = _dataMovements.SprintMaxMult;
    }

    private void OnWalk()
    {
        _dataMovements.SprintMult = 1;
    }

    private void OnGravity(Vector3 direction)
    { 
        direction.y -= _dataMovements.Gravity;

        _controller.Move(direction * Time.deltaTime);
    }

    private void OnRaycast()
    {
        RaycastHit raycastHit;
        _hit = Physics.Raycast(_camera.transform.position, _camera.transform.forward, out raycastHit, _distanceInteract, _activeLayer);

        if(_hit == true)
        {
            _interactableObj = null;
            _interactableObj = raycastHit.transform.gameObject;
        }
    }

    private void OnInteract()
    {
        IInteract interact = _interactableObj.GetComponent<IInteract>();
        if(_hit == true && interact != null)
        {
            interact.Enter();
        }
    }

    private void OnPickUp()
    {
        IAction action = _interactableObj.GetComponent<IAction>();

        if (_hit == true && action != null)
        {
            action.Enter(_camera.transform);
            InputManager.Instance.Interaction += OnDrop;
            InputManager.Instance.Throw += OnThrow;
            InputManager.Instance.Interaction -= OnPickUp;
        }
    }

    public void OnDrop()
    {
        IAction action = _interactableObj.GetComponent<IAction>();
        action.Exit();
        InputManager.Instance.Interaction += OnPickUp;
        InputManager.Instance.Interaction -= OnDrop;
        InputManager.Instance.Throw -= OnThrow;
    }

    private void OnThrow()
    {
        IAction action = _interactableObj.GetComponent<IAction>();
        Pickable pickable = _interactableObj.GetComponent<Pickable>();

        action.Exit();
        pickable.Rigidbody.AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);

        InputManager.Instance.Interaction += OnPickUp;
        InputManager.Instance.Interaction -= OnDrop;
        InputManager.Instance.Throw -= OnThrow;
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.Player -= OnRaycast;
        GameLoopManager.Instance.Pause -= IsPaused;
        InputManager.Instance.Movement -= OnMovements;
        InputManager.Instance.Idle -= OnIdle;
        InputManager.Instance.Interaction -= OnPickUp;
        InputManager.Instance.Interaction -= OnInteract;
        InputManager.Instance.PressSprint -= OnSprint;
        InputManager.Instance.ReleaseSprint -= OnWalk;
    }
}
