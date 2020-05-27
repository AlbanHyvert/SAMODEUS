using Engine.Loading;
using UnityEngine.SceneManagement;

public class MainMenuState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("MENU");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("MENU");
    }
}
