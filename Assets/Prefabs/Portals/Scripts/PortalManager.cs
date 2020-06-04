using Engine.Singleton;

public class PortalManager : Singleton<PortalManager>
{
    public Portal PortalVertumne { get; set; }
    public Portal PortalGCF { get; set; }

    public PortalNoScreen PortalNSA { get; set; }
    public PortalNoScreen PortalNSB { get; set; }
}
