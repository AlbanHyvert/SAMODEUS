using UnityEngine.SceneManagement;

public class DevState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("DEV");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("DEV");
    }
}
