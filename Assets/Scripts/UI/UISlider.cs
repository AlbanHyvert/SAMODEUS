﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [SerializeField] private Slider _verticalSlider = null;
    [SerializeField] private Slider _horizontalSlider = null;
    [SerializeField] private Slider _musicSlider = null;
    [SerializeField] private Slider _dialogueSlider = null;

    [SerializeField] private TextMeshProUGUI _vertValue = null;
    [SerializeField] private TextMeshProUGUI _HoriValue = null;

    private float _musicValue = 0.0f;
    private float _dialsValue = 0.0f;

    private void Start()
    {
        _musicSlider.value = _musicSlider.maxValue;
        _dialogueSlider.value = _dialogueSlider.maxValue;
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
        _musicValue = _musicSlider.value;

        if(PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.MusicAudioSource.volume = _musicValue;
    }

    public void OnDialsValueChanged()
    {
        _dialsValue = _dialogueSlider.value;

        if(PlayerManager.Instance.Player != null)
            PlayerManager.Instance.Player.DialsAudioSource.volume = _dialsValue;
    }
}
