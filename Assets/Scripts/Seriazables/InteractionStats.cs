using UnityEngine;

[System.Serializable]
public class InteractionStats
{
    [SerializeField] private int _maxDistanceInteract = 10;
    [SerializeField] private int _throwForce = 100;
    [SerializeField] private LayerMask _activeLayer = 0;

    public int MaxDistanceInteract { get { return _maxDistanceInteract; } }
    public int ThrowForce { get { return _throwForce; } }
    public LayerMask ActiveLayer { get { return _activeLayer; } }
}
