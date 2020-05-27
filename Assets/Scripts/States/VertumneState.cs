using Engine.Loading;
using System.Collections.Generic;

public class VertumneState : IGameStates
{
    private List<string> _nameList = new List<string>();

    void IGameStates.Enter()
    {
        _nameList.Add("Vertumne_2_2");
        _nameList.Add("Vertumne_2_1");

        LoadingManager.Instance.LoadScene(_nameList.ToArray());
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.UnloadScene(_nameList.ToArray());
        _nameList.Clear();
    }
}
