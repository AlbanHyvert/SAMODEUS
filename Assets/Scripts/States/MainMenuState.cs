using UnityEngine.SceneManagement;

public class MainMenuState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadScene("MENU");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("MENU");
    }
}
