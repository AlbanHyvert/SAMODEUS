using UnityEngine.SceneManagement;

public class PreloadState : IGameStates
{
    void IGameStates.Enter()
    {
        SceneManager.LoadScene("PRELOAD");
    }

    void IGameStates.Exit()
    {
        
    }
}