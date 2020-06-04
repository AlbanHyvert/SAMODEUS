using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _hudPause = null;
    [SerializeField] private GameObject _hudSettings = null;
    [SerializeField] private GameObject _hudCredits = null;

    private void Start()
    {
        _hudPause.SetActive(false);
        _hudSettings.SetActive(false);
        _hudCredits.SetActive(false);
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
            _hudSettings.SetActive(false);
            _hudCredits.SetActive(false);
        }
    }

    public void Resume()
    {
        GameLoopManager.Instance.IsPaused = false;
    }

    public void Restart()
    {
        GameLoopManager.Instance.IsPaused = false;
        GameManager.Instance.ChangeState(GameManager.GameState.LOADING);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
