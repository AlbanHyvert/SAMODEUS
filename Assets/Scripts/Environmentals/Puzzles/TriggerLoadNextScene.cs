using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TriggerLoadNextScene : MonoBehaviour
{
    [SerializeField] private GameManager.GameState _ChoosenScene = GameManager.GameState.GAME;
    [SerializeField] private bool _specialItem = false;
    [SerializeField] private MeshRenderer _screen = null;

    private RenderTexture _viewTexture = null;
    private Camera _camera = null;

    public Camera Camera { get { return _camera; } set { _camera = value; } }

    private void Start()
    {
        Render();
        GameLoopManager.Instance.Puzzles += OnUpdate;
    }

    private void OnUpdate()
    {
        Render();
    }

    private void CreateViewTexture()
    {
        if (_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
        {
            if (_viewTexture != null)
            {
                _viewTexture.Release();
            }
            _viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            //Render the view from the portal camera to the view texture
            _camera.targetTexture = _viewTexture;
        }
    }

    private static bool VisibleFromCamera(Renderer renderer, Camera camera)
    {
        Plane[] frustrumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(frustrumPlanes, renderer.bounds);
    }

    // Called just before player camera is rendered
    public void Render()
    {
        if (!VisibleFromCamera(_screen, PlayerManager.Instance.Player.Camera))
        {
            return;
        }

        _screen.material.SetTexture("_MainTex", _viewTexture);
        _screen.enabled = false;

        CreateViewTexture();

        _screen.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        Pickable pickable = other.GetComponent<Pickable>();

        if(player != null)
        {
            PlayerManager.Instance.DestroyPlayer();
            GameManager.Instance.ChoosenScene = _ChoosenScene;
            GameManager.Instance.ChangeState(GameManager.GameState.LOADING);
        }

        if(pickable != null && _specialItem == true)
        {
            Rigidbody rb = transform.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                transform.SetParent(null);
            }
        }
    }
}
