using UnityEngine.SceneManagement;

public class VertumneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("Vertumne_1");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("Vertumne_1");
    }
}
