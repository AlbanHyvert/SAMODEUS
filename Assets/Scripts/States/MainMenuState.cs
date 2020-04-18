using UnityEngine.SceneManagement;

public class MainMenuState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("MENU");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("MENU");
    }
}
