using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource _menuMusicAudio = null;
    [SerializeField] private AudioSource _menuDialsAudio = null;

    public AudioSource MenuMusicAudio { get { return _menuMusicAudio; } }
    public AudioSource MenuDialsAudio { get { return _menuDialsAudio; } }

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
