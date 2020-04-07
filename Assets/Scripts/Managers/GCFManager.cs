using Engine.Singleton;
using UnityEngine;

public class GCFManager : Singleton<GCFManager>
{
    [SerializeField] private DataGCFRotatingCube _dataGCFRotatingCube = null;
    [SerializeField] private DataGCFMovingCube _dataGCFMovingCube = null;

    private float _actionRayonRotating = 0.0f;
    private float _actionRayonMoving = 0.0f;

    public DataGCFRotatingCube DataGCFRotatingCube { get { return _dataGCFRotatingCube; } }
    public DataGCFMovingCube DataGCFMovingCube { get { return _dataGCFMovingCube; } }

    public float ActionRayonRotating { get { return _actionRayonRotating; } set { _actionRayonRotating = value; } }
    public float ActionRayonMoving { get { return _actionRayonMoving; } set { _actionRayonMoving = value; } }

    private void Start()
    {
        ActionRayonMoving = _dataGCFMovingCube.ActionRayon;
        ActionRayonRotating = _dataGCFRotatingCube.ActionRayon;
    }
}
