using Engine.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : Singleton<PortalManager>
{
    public Portal PortalA { get; set; }
    public Portal PortalB { get; set; }

    public PortalNoScreen PortalNSA { get; set; }
    public PortalNoScreen PortalNSB { get; set; }

    public enum PortalID
    {
        PORTAL_A,
        PORTAL_B
    }

    private void Update()
    {
        Debug.Log("Portal A : " + PortalA);
        Debug.Log("Portal B : " + PortalB);
    }
}
