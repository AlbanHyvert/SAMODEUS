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
        _musicSlider.value = PlayerManager.Instance.MusicVolume;
        _dialogueSlider.value = PlayerManager.Instance.DialVolume;

        if (PlayerManager.Instance.Player != null)
        {
            float musicVolume = _musicSlider.value / 100;
            float dialVolume = _dialogueSlider.value / 100;

            PlayerManager.Instance.Player.MusicAudioSource.volume = musicVolume;
            PlayerManager.Instance.Player.DialsAudioSource.volume = dialVolume;
        }
        else
        {
            PlayerManager.Instance.DialVolume = _dialogueSlider.value;
            PlayerManager.Instance.MusicVolume = _musicSlider.value;

            if(_mainMenu != null)
            {
                float musicVolume = _musicSlider.value / 100;
                float dialVolume = _dialogueSlider.value / 100;

                _mainMenu.MenuMusicAudio.volume = musicVolume;
                _mainMenu.MenuDialsAudio.volume = dialVolume;
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

        float musicVolume = _musicSlider.value / 100;

        if (PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.MusicAudioSource.volume = musicVolume;
        else
        {
            if (_mainMenu != null)
            {
                _mainMenu.MenuMusicAudio.volume = musicVolume;
            }
        }
    }

    public void OnDialsValueChanged()
    {
        PlayerManager.Instance.DialVolume = _dialogueSlider.value;

        float dialVolume = _dialogueSlider.value / 100;

        if (PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.DialsAudioSource.volume = dialVolume;
        else
        {
            if (_mainMenu != null)
            {
                _mainMenu.MenuMusicAudio.volume = dialVolume;
            }
        }
    }
}
