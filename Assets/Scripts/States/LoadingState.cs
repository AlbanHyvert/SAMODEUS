using Engine.Loading;
using System.Collections.Generic;

public class LoadingState : IGameStates
{
    private List<string> _nameList = new List<string>();

    void IGameStates.Enter()
    {
        _nameList.Add("LOADING");
        LoadingManager.Instance.LoadScene("LOADING");
    }

    void IGameStates.Exit()
    {
        LoadingManager.Instance.SceneToUnload = "LOADING";
    }
}
