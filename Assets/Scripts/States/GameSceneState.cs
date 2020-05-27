using System.Collections.Generic;
using Engine.Loading;

public class GameSceneState : IGameStates
{
    private List<string> _nameList = new List<string>();

    void IGameStates.Enter()
    {
        _nameList.Add("GAME");
        _nameList.Add("Vertumne_2_2");
        _nameList.Add("Vertumne_2_1");
        _nameList.Add("GCF_1");

        //SceneAsyncManager.Instance.LoadScenes(_nameList.ToArray());
        LoadingManager.Instance.LoadScene(_nameList.ToArray());
    }

    void IGameStates.Exit()
    {
        //SceneAsyncManager.Instance.UnloadScenes(_nameList.ToArray());
        LoadingManager.Instance.UnloadScene(_nameList.ToArray());
        _nameList.Clear();
    }
}
