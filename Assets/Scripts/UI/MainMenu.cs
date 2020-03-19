using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameLoopManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.LOADING);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
