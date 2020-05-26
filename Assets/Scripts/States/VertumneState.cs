using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VertumneState : IGameStates
{
    private List<string> _nameList = new List<string>();

    void IGameStates.Enter()
    {
        _nameList.Add("Vertumne_2_1");
        _nameList.Add("Vertumne_2_2");

        SceneAsyncManager.Instance.LoadScenes(_nameList.ToArray());
    }

    void IGameStates.Exit()
    {
        SceneAsyncManager.Instance.UnloadScenes(_nameList.ToArray());
        _nameList.Clear();
    }
}
