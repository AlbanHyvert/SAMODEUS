using UnityEngine;

public interface IAction
{
    void Enter(Transform obj);

    void Exit();

    void DestroySelf(Transform parent);
}
