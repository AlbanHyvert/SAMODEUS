using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Portal : MonoBehaviour
{
    [SerializeField] private bool _shouldBeDestroyed = false;
    [SerializeField] private Portal _linkedPortal = null;
    [SerializeField] private MeshRenderer _screen = null;
    [SerializeField] private bool _shouldShake = false;
    [SerializeField] private Portal_ENUM _portalID = Portal_ENUM.VERTUMNE;


    private Camera _playerCamera = null;
    private Camera _portalCamera = null;
    private RenderTexture _viewTexture = null;
    private List<PortalTraveller> trackedTravellers = null;

    public Portal LinkedPortal { get { return _linkedPortal; } set { _linkedPortal = value;} }

    public MeshRenderer MeshScreen { get { return _screen; } }

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        _portalCamera = GetComponentInChildren<Camera>();
        _portalCamera.enabled = false;

        if (_portalID == Portal_ENUM.VERTUMNE)
        {
            PortalManager.Instance.PortalVertumne = this;
            gameObject.SetActive(false);
        }
        else if (_portalID == Portal_ENUM.GCF)
        {
            PortalManager.Instance.PortalGCF = this;
        }
    }

    private void Start()
    {
        if(_linkedPortal == null)
        {
            if(_portalID == Portal_ENUM.GCF)
            {
                if(PortalManager.Instance.PortalVertumne != null)
                {
                    PortalManager.Instance.PortalVertumne.gameObject.SetActive(true);
                    _linkedPortal = PortalManager.Instance.PortalVertumne;
                }

            }
            else if (_portalID == Portal_ENUM.VERTUMNE)
            {
                if(PortalManager.Instance.PortalGCF != null)
                {
                    _linkedPortal = PortalManager.Instance.PortalGCF;
                }
            }
        }

        trackedTravellers = new List<PortalTraveller>();
        GameLoopManager.Instance.Player += CheckPlayerStatus;
    }

    private void CheckPlayerStatus()
    {
        if(PlayerManager.Instance.Player != null)
        {
            _playerCamera = PlayerManager.Instance.Player.CameraController.Camera;
            Render();
            GameLoopManager.Instance.Puzzles += OnUpdate;
            GameLoopManager.Instance.Player -= CheckPlayerStatus;
        }
    }

    private void OnUpdate()
    {
        Render();
        if(trackedTravellers != null)
        {
            for (int i = 0; i < trackedTravellers.Count; i++)
            {
                PortalTraveller traveller = trackedTravellers[i];
                Transform travellerT = traveller.transform;

                Vector3 offsetFromPortal = travellerT.position - transform.position;

                int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.PreviousOffsetFromPortal, transform.forward));

                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    Matrix4x4 m = _linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                    traveller.Teleport(transform, _linkedPortal.transform, m.GetColumn(3), m.rotation, _shouldShake);

                    // Can't rely on OnTriggerEnter/Exit to be called next frame because it depends on when FixedUpdate runs
                    _linkedPortal.OnTravellerEnterPortal(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;

                    if(_shouldBeDestroyed == true)
                    {
                        Destroy(this);
                    }
                }
                else
                {
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }
            }
        }

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
                _portalCamera.targetTexture = _viewTexture;
                // Display the view texture on the screen of the linked portal
                _linkedPortal._screen.material.SetTexture("_MainTex", _viewTexture);
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
        if(_playerCamera != null && _screen != null && _linkedPortal._screen != null)
        {
            if (!VisibleFromCamera(_linkedPortal._screen, _playerCamera))
            {
                return;
            }

            _linkedPortal._screen.material.SetTexture("_MainTex", _viewTexture);
            _screen.enabled = false;

            CreateViewTexture();

            // Make portal cam position and rotation the same relative to this portal as player cam relative linked portal
            Matrix4x4 m = transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix * _playerCamera.transform.localToWorldMatrix;

            if (_portalCamera != null)
            {
                _portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

                //Render Camera
                _portalCamera.Render();
            }
        }
        //ProtectScreenFromClipping();
        if(_screen != null)
            _screen.enabled = true; 
    }

    private void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if(traveller != null)
        {
            if (!trackedTravellers.Contains(traveller))
            {
                traveller.EnterPortalThreshold();
                traveller.PreviousOffsetFromPortal = traveller.transform.position - transform.position;
                trackedTravellers.Add(traveller);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PortalTraveller traveller = other.GetComponent<PortalTraveller>();
        if(traveller != null)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PortalTraveller traveller = other.GetComponent<PortalTraveller>();
        if (traveller && trackedTravellers.Contains(traveller))
        {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }

    private void OnDestroy()
    {
        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
            GameLoopManager.Instance.Player -= CheckPlayerStatus;
        }
    }
}
