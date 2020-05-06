using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _hudPause = null;

    private void Start()
    {
        _hudPause.SetActive(false);
        GameLoopManager.Instance.Pause += IsPaused;
    }
    private void IsPaused(bool pause)
    {
        if(pause == true)
        {
            if (_hudPause != null)
            {
                _hudPause.SetActive(true);
            }
        }
        else
        {
            _hudPause.SetActive(false);
        }
    }

    public void Resume()
    {
        GameLoopManager.Instance.IsPaused = false;
    }

    public void Restart()
    {
        PlayerManager.Instance.DestroyPlayer();
        GameLoopManager.Instance.IsPaused = false;
        GameManager.Instance.ChangeState(GameManager.GameState.LOADING);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
