using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GameLoopManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
