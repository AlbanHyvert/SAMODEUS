using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [SerializeField] private MainMenu _mainMenu = null;

    [SerializeField] private Slider _verticalSlider = null;
    [SerializeField] private Slider _horizontalSlider = null;
    [SerializeField] private Slider _musicSlider = null;
    [SerializeField] private Slider _dialogueSlider = null;

    [SerializeField] private TextMeshProUGUI _vertValue = null;
    [SerializeField] private TextMeshProUGUI _HoriValue = null;

    private void Start()
    {
        _musicSlider.value = _musicSlider.maxValue;
        _dialogueSlider.value = _dialogueSlider.maxValue;

        if (PlayerManager.Instance.Player != null)
        {
            PlayerManager.Instance.Player.MusicAudioSource.volume = PlayerManager.Instance.MusicVolume / 100;
            PlayerManager.Instance.Player.DialsAudioSource.volume = PlayerManager.Instance.DialsVolume / 100;
        }
        else
        {
            if(_mainMenu != null)
            {
                _mainMenu.MenuMusicAudio.volume = _musicSlider.value;
                _mainMenu.MenuDialsAudio.volume = _dialogueSlider.value;
            }
        }

        _verticalSlider.value = InputManager.Instance.VerticalSensitivity;
        _horizontalSlider.value = InputManager.Instance.HorizontalSensitivity;
    }

    public void OnVerticalValueChanged()
    {
        InputManager.Instance.VerticalSensitivity = (int)_verticalSlider.value;
        _vertValue.text = InputManager.Instance.VerticalSensitivity.ToString();
    }

    public void OnHorizontalValueChanged()
    {
        InputManager.Instance.HorizontalSensitivity = (int)_horizontalSlider.value;
        _HoriValue.text = InputManager.Instance.HorizontalSensitivity.ToString();
    }

    public void OnMusicValueChanged()
    {
        PlayerManager.Instance.MusicVolume = _musicSlider.value;

        if(PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.MusicAudioSource.volume = PlayerManager.Instance.MusicVolume / 100;
        else
        {
            if (_mainMenu != null)
            {
                _mainMenu.MenuMusicAudio.volume = PlayerManager.Instance.MusicVolume / 100;
            }
        }
    }

    public void OnDialsValueChanged()
    {
        PlayerManager.Instance.DialsVolume = _dialogueSlider.value;

        if(PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.DialsAudioSource.volume = PlayerManager.Instance.DialsVolume / 100;
        else
        {
            if (_mainMenu != null)
            {
                _mainMenu.MenuMusicAudio.volume = PlayerManager.Instance.DialsVolume / 100;
            }
        }
    }
}
