using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameManager.GameState _gameState = GameManager.GameState.GAME;
    private void Start()
    {
        GameLoopManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnStart()
    {
        GameManager.Instance.ChangeState(_gameState);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
