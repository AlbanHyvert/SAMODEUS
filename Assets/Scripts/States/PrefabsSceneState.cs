using UnityEngine.SceneManagement;

public class PrefabsSceneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneAsyncManager.Instance.LoadScene("Prefab_Vertumne");
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScene("Prefab_Vertumne");
    }
}
