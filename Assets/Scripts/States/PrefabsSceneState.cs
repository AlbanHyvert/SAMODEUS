using UnityEngine.SceneManagement;

public class PrefabsSceneState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadSceneAsync("Prefab_Vertumne");
    }

    void IGameStates.Exit()
    {
        SceneManager.UnloadSceneAsync("Prefab_Vertumne");
    }
}
