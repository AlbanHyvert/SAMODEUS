using UnityEngine.SceneManagement;

public class GCF_1State : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadScene("GCF_1");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("GCF_1");
    }
}
