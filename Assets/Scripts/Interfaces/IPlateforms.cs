using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlateforms
{
    void Init(RotatingCube rotatingCube = null, Transform target = null);

    void Enter();

    void Tick();

    void Exit();
}
