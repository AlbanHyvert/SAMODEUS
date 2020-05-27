using Engine.Loading;

public class GCFState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("GCF_1");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("GCF_1");
    }
}
