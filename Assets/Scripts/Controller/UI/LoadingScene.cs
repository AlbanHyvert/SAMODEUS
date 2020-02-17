using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private GameManager.GameState _gameState = GameManager.GameState.GAME;
    private void Start()
    {
        GameLoopManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.Instance.ChangeState(_gameState);
    }
}
