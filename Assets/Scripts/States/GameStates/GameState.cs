using UnityEngine.SceneManagement;

public class GameState : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadScene("Game");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("Game");
    }
}
