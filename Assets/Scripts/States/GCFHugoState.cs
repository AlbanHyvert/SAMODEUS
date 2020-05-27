using Engine.Loading;

public class GCFHugoState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("GCF_HUGO");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("GCF_HUGO");
    }
}
