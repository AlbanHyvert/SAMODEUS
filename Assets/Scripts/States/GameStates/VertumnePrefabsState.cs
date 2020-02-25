using UnityEngine.SceneManagement;

public class VertumnePrefabsState : IGameState
{
    void IGameState.Enter()
    {
        GameLoopManager.Instance.IsPaused = false;
        SceneManager.LoadSceneAsync("Prefab_Vertumne");
    }

    void IGameState.Exit()
    {
        SceneManager.UnloadSceneAsync("Prefab_Vertumne");
    }
}
