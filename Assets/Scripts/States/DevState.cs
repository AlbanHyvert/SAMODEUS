using UnityEngine.SceneManagement;

public class DevState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("DEV");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("DEV");
    }
}
