using UnityEngine.SceneManagement;

public class NE1State : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("NE_1");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("NE_1");
    }
}
