using UnityEngine.SceneManagement;

public class PreloadState : IGameState
{
    void IGameState.Enter()
    {
        SceneManager.LoadSceneAsync("Preload");
        GameManager.Instance.ChangeState(GameManager.GameState.MAINMENU);
    }

    void IGameState.Exit()
    {

    }
}
