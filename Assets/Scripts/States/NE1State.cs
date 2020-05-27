using Engine.Loading;

public class NE1State : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("NE_1");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("NE_1");
    }
}
