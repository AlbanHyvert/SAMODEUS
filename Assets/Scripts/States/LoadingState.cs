using UnityEngine;

public class LoadingState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("LOADING");
    }

    void IGameStates.Exit()
    {
        Debug.Log("unload");
        SceneAsyncManager.Instance.UnloadScene("LOADING");
    }
}
