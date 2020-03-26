using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject _hudPause = null;
    [SerializeField] private Sprite[] _vertumneSprites = null;
    [SerializeField] private Sprite[] _GCFSprites = null;
    [SerializeField] private Image[] _pauseButtonsImage = null;

    private void Start()
    {
        _hudPause.SetActive(false);
        GameLoopManager.Instance.Pause += IsPaused;
    }

    private void IsPaused(bool pause)
    {
        PlayerController playerController = PlayerManager.Instance.Player;

        if(pause == true)
        {
            if (_hudPause != null)
            {
                _hudPause.SetActive(true);
            }

            if(playerController.WorldTaged == PlayerManager.WorldTag.VERTUMNE)
            {
                for (int i = 0; i < _pauseButtonsImage.Length; i++)
                {
                    _pauseButtonsImage[i].sprite = _vertumneSprites[i];
                }
            }
            else if(playerController.WorldTaged == PlayerManager.WorldTag.GCF)
            {
                for (int i = 0; i < _pauseButtonsImage.Length; i++)
                {
                    _pauseButtonsImage[i].sprite = _GCFSprites[i];
                }
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
