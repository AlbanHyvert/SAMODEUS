using UnityEngine.SceneManagement;

public class LoadingState : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadScene("LoadingScene");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("LoadingScene");
    }
}
