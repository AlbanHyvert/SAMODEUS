using UnityEngine.SceneManagement;

public class GameSceneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("GAME");
        SceneManager.LoadSceneAsync("Vertumne_1", LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync("GCF_1", LoadSceneMode.Additive);
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("GAME");
        SceneManager.UnloadSceneAsync("GCF_1");
        SceneManager.UnloadSceneAsync("Vertumne_1");
    }
}
