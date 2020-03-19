using UnityEngine.SceneManagement;

public class GCFState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("GCF_1");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("GCF_1");
    }
}
