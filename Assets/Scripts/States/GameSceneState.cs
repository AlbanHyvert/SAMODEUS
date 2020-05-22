using System.Collections.Generic;


public class GameSceneState : IGameStates
{
    private List<string> _nameList = new List<string>();

    void IGameStates.Enter()
    {
        _nameList.Add("GAME");
        _nameList.Add("Vertumne_1");
        _nameList.Add("GCF_1");

        SceneAsyncManager.Instance.LoadScenes(_nameList.ToArray());
    }

    void IGameStates.Exit()
    {
        /*SceneAsyncManager.Instance.UnloadScenes(_nameList.ToArray());
        _nameList.Clear();*/
    }
}
