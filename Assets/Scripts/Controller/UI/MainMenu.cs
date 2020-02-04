using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.GAME);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
