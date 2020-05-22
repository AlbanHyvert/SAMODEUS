public class LoadingState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("LOADING");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("LOADING");
    }
}
