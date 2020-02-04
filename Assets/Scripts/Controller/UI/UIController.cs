using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField ,Header("GO Hud")] private GameObject _hud = null;
    [SerializeField ,Header("GO PauseHud")] private GameObject _pausedHud = null;
    [SerializeField ,Header("Setting Hud")] private GameObject _settingHud = null;

    private void Start()
    {
        GameLoopManager.Instance.Pause += IsPaused;
        _hud.SetActive(GameLoopManager.Instance.IsPaused);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = GameLoopManager.Instance.IsPaused;
        GameLoopManager.Instance.GetCanvas += OnUpdate;
    }

    private void IsPaused(bool pause)
    {
        if(pause == false)
        {
            GameLoopManager.Instance.GetCanvas -= OnUpdate;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            GameLoopManager.Instance.GetCanvas += OnUpdate;
        }
        _hud.SetActive(pause);
    }

    private void OnUpdate()
    {
        if(GameLoopManager.Instance.IsPaused == true)
        {
            Cursor.visible = GameLoopManager.Instance.IsPaused;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void OnRestart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _hud.SetActive(GameLoopManager.Instance.IsPaused);
        GameManager.Instance.ChangeState(GameManager.GameState.GAME);
    }

    public void OnMenu()
    {
        GameLoopManager.Instance.IsPaused = false;
        _hud.SetActive(GameLoopManager.Instance.IsPaused);
        GameManager.Instance.ChangeState(GameManager.GameState.MAINMENU);
    }

    public void OnQuit()
    {
        Application.Quit();
    }

    public void OnResume()
    {
        GameLoopManager.Instance.IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        _pausedHud.SetActive(!GameLoopManager.Instance.IsPaused);
        _settingHud.SetActive(GameLoopManager.Instance.IsPaused);
        _hud.SetActive(GameLoopManager.Instance.IsPaused);
    }

    private void OnDestroy()
    {
        GameLoopManager.Instance.GetCanvas -= OnUpdate;
    }
}
