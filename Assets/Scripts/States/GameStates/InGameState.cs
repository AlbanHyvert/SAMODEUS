using UnityEngine.SceneManagement;
public class InGameState : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadSceneAsync("Game");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("Game");
    }
}
