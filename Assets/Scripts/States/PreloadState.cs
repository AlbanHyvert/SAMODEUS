using UnityEngine.SceneManagement;

public class PreloadState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("PRELOAD");
    }

    void IGameStates.Exit()
    {
        
    }
}