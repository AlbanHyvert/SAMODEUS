using Engine.Singleton;
using UnityEngine;

public class GCFManager : Singleton<GCFManager>
{
    [SerializeField] private DataGCFRotatingCube _dataGCFRotatingCube = null;
    [SerializeField] private DataGCFMovingCube _dataGCFMovingCube = null;

    public DataGCFRotatingCube DataGCFRotatingCube { get { return _dataGCFRotatingCube; } }
    public DataGCFMovingCube DataGCFMovingCube { get { return _dataGCFMovingCube; } }
}
