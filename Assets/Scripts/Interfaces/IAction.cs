using UnityEngine;

public interface IAction
{
    void Enter(PlayerController player = null);

    void Exit();

    void DestroySelf(Transform transform = null);
}
