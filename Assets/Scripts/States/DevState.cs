using Engine.Loading;

public class DevState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("DEV");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("DEV");
    }
}
