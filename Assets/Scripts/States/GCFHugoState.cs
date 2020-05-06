public class GCFHugoState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("GCF_HUGO");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("GCF_HUGO");
    }
}
