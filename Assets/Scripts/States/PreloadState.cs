using Engine.Loading;

public class PreloadState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("PRELOAD");
    }

    void IGameStates.Exit()
    {
        
    }
}