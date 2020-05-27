using Engine.Loading;

public class PrefabsSceneState : IGameStates
{
    void IGameStates.Enter()
    {
        LoadingManager.Instance.LoadScene("Prefab_Vertumne");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene("Prefab_Vertumne");
    }
}
