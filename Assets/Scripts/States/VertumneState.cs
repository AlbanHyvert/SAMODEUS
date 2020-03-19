using UnityEngine.SceneManagement;

public class VertumneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("Vertumne_1");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("Vertumne_1");
    }
}
