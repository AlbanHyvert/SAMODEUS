using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Portal _linkedPortal = null;
    [SerializeField] private MeshRenderer _screen = null;
    private Camera _playerCamera = null;
    private Camera _portalCamera = null;
    private RenderTexture _viewTexture = null;
    private List<PortalTraveller> trackedTravellers = null;

    private void Awake()
    {
        if(_playerCamera == null)
        {
            _playerCamera = Camera.main;
            
            if(_playerCamera == null)
            {
                _playerCamera = PlayerManager.Instance.Player.PlayerCamera;
            }
        }

        _portalCamera = GetComponentInChildren<Camera>();
        _portalCamera.enabled = false;
        trackedTravellers = new List<PortalTraveller>();
        Render();
    }

    private void LateUpdate()
    {
        Render();
        for (int i = 0; i < trackedTravellers.Count; i++)
        {
            PortalTraveller traveller = trackedTravellers[i];
            Transform travellerT = traveller.transform;

            Vector3 offsetFromPortal = travellerT.position - transform.position;
            int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
            int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.PreviousOffsetFromPortal, transform.forward));
            // Teleport the traveller if it has crossed from one side of the portal to the other
            if(portalSide != portalSideOld)
            {
                Matrix4x4 m = _linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                traveller.Teleport(transform, _linkedPortal.transform, m.GetColumn(3), m.rotation);

                // Can't rely on OnTriggerEnter/Exit to be called next frame because it depends on when FixedUpdate runs
                _linkedPortal.OnTravellerEnterPortal(traveller);
                trackedTravellers.RemoveAt(i);
                i--;
            }
            else
            {
                traveller.PreviousOffsetFromPortal = offsetFromPortal;
            }
        }
    }

   /* private void ProtectScreenFromClipping()
    {
        float halfHeight = _playerCamera.nearClipPlane * Mathf.Tan(_playerCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * _playerCamera.aspect;
        float dstToNearClipPlaneCorner = new Vector3(halfWidth, halfHeight, _playerCamera.nearClipPlane).magnitude;

        Transform screenT = _screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot(transform.forward, transform.position - _playerCamera.transform.position) > 0;
        screenT.localScale = new Vector3(screenT.localScale.x, screenT.localScale.y, dstToNearClipPlaneCorner);
        screenT.localPosition = Vector3.forward * dstToNearClipPlaneCorner * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
    }*/

    private void CreateViewTexture()
    {
        if(_viewTexture == null || _viewTexture.width != Screen.width || _viewTexture.height != Screen.height)
        {
            if(_viewTexture != null)
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
        if(!VisibleFromCamera(_linkedPortal._screen, _playerCamera))
        {
            var testTexture = new Texture2D(1, 1);
            testTexture.SetPixel(0, 0, Color.red);
            testTexture.Apply();
            _linkedPortal._screen.material.SetTexture("_MainTex", testTexture);
            return;
        }

        _linkedPortal._screen.material.SetTexture("_MainTex", _viewTexture);
        _screen.enabled = false;
        CreateViewTexture();

        // Make portal cam position and rotation the same relative to this portal as player cam relative linked portal
        Matrix4x4 m = transform.localToWorldMatrix * _linkedPortal.transform.worldToLocalMatrix * _playerCamera.transform.localToWorldMatrix;
        _portalCamera.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        //Render Camera
        _portalCamera.Render();
        //ProtectScreenFromClipping();
        _screen.enabled = true; 
    }

    private void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if(!trackedTravellers.Contains(traveller))
        {
            traveller.EnterPortalThreshold();
            traveller.PreviousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if(traveller)
        {
            OnTravellerEnterPortal(traveller);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller && trackedTravellers.Contains(traveller))
        {
            traveller.ExitPortalThreshold();
            trackedTravellers.Remove(traveller);
        }
    }
}
