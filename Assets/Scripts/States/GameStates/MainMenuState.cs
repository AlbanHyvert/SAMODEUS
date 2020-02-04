using UnityEngine.SceneManagement;

public class MainMenuState : IGameState
{
    void IGameState.Enter()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
