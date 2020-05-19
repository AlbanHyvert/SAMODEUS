using System.Collections.Generic;
using UnityEngine;

public class PortalNoScreen : MonoBehaviour
{
    [SerializeField] private PortalNoScreen _linkedPortal = null;
    [SerializeField] private bool _shouldShake = false;
    [SerializeField] private PortalManager.PortalID _portalID = PortalManager.PortalID.PORTAL_A;

    private List<PortalTraveller> trackedTravellers = null;

    private void Start()
    {
        trackedTravellers = new List<PortalTraveller>();


        if (_portalID == PortalManager.PortalID.PORTAL_A)
        {
            PortalManager.Instance.PortalNSA = this;
        }
        else if (_portalID == PortalManager.PortalID.PORTAL_B)
        {
            PortalManager.Instance.PortalNSB = this;
        }

        if (_linkedPortal == null && _portalID == PortalManager.PortalID.PORTAL_A)
        {
            _linkedPortal = PortalManager.Instance.PortalNSB;
        }
        else if (_linkedPortal == null && _portalID == PortalManager.PortalID.PORTAL_B)
        {
            _linkedPortal = PortalManager.Instance.PortalNSA;
        }
    }

    private void Update()
    {
        if (trackedTravellers != null)
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
                }
                else
                {
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }
            }
        }

    }

    private void OnTravellerEnterPortal(PortalTraveller traveller)
    {
        if (!trackedTravellers.Contains(traveller))
        {
            traveller.EnterPortalThreshold();
            traveller.PreviousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add(traveller);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PortalTraveller traveller = other.GetComponent<PortalTraveller>();
        if (traveller)
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

}
