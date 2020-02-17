using UnityEngine.SceneManagement;

public class VertumneState : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadSceneAsync("Vertumne_1");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("Vertumne_1");
    }
}
