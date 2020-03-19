using UnityEngine.SceneManagement;

public class NE1State : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("NE_1");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("NE_1");
    }
}
