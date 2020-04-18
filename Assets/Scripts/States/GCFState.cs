using UnityEngine.SceneManagement;

public class GCFState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("GCF_1");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("GCF_1");
    }
}
