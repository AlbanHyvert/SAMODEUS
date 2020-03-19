using UnityEngine.SceneManagement;

public class GameSceneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("GAME");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("GAME");
    }
}
