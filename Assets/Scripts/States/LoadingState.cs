using UnityEngine.SceneManagement;

public class LoadingState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("LOADING");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("LOADING");
    }
}
