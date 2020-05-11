using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class DialBoxController : MonoBehaviour
{
    [SerializeField ,Header("TmPro Text Zone")] private TextMeshProUGUI _text = null;
    [SerializeField ,Header("Blank Time Between Text")] private float _blankThreshold = 0.1f;
    [SerializeField] private TextMeshProUGUI _buttonsToPressText = null;

    private AudioSource _source = null;
    private float _timeStamp = 0.0f;
    private List<TextDialBoxData> _textDialBoxDataList = null;
    private List<VoiceDialBoxData> _voiceDialBoxDataList = null;
    private bool _timerIsStarted = false;

    public bool TimerIsStarted { get { return _timerIsStarted; } }

    private void Start()
    {
        if (_buttonsToPressText != null)
            _buttonsToPressText.text = string.Empty;

        NarrativeManager.Instance.DialBoxController = this;
        _text.text = string.Empty;
        _source = PlayerManager.Instance.Player.DialsAudioSource;
        NarrativeManager.Instance.OnCallNarration += OnTriggerNarration;
        GameLoopManager.Instance.Pause += IsPause;
        GameLoopManager.Instance.UI += OnUpdate;
    }

    private void IsPause(bool pause)
    {
        if(pause == false)
        {
            GameLoopManager.Instance.UI += OnUpdate;
            InputManager.Instance.PassDialogue += PassDials;
        }
        else
        {
            GameLoopManager.Instance.UI -= OnUpdate;
            InputManager.Instance.PassDialogue -= PassDials;
        }
    }

    private void OnTriggerNarration(VoiceDialBoxData[] voiceDials, TextDialBoxData[] textDials)
    {
        _voiceDialBoxDataList = new List<VoiceDialBoxData>(voiceDials);
        _textDialBoxDataList = new List<TextDialBoxData>(textDials);
        TriggerFirstAudioDialsElement();
        TriggerFirstTextDialsElement();
        InputManager.Instance.PassDialogue += PassDials;
    }

    private void TriggerFirstAudioDialsElement()
    {
        if (_text.text == string.Empty)
        {
            _text.text = _textDialBoxDataList[0].Text;
            _timeStamp = Time.time + _textDialBoxDataList[0].LifeTime;
            _timerIsStarted = true;
            _textDialBoxDataList.RemoveAt(0);         
        }
    }

    private void TriggerFirstTextDialsElement()
    {
        _source.PlayOneShot(_voiceDialBoxDataList[0].Clip);
        Debug.Log("SoundsPlaying");
        _voiceDialBoxDataList.RemoveAt(0);
    }

    private void OnUpdate()
    {
        if(_timerIsStarted == true)
        {
            _buttonsToPressText.text = InputManager.Instance.DataKeycode.KeyDialogue.ToString();

            if (Time.time >= _timeStamp)
            {
                if(_textDialBoxDataList!= null && _textDialBoxDataList.Count > 0)
                {
                    TriggerFirstTextDialsElement();
                }
                else
                {
                    _textDialBoxDataList = null;
                }

                if(_voiceDialBoxDataList != null && _voiceDialBoxDataList.Count > 0)
                {
                    TriggerFirstAudioDialsElement();
                }
                else
                {
                    _voiceDialBoxDataList = null;
                }

                if(_voiceDialBoxDataList == null && _textDialBoxDataList == null && _source.isPlaying == false)
                {
                    _timerIsStarted = false;
                }
            }
            else if(Time.time >= _timeStamp - _blankThreshold)
            {
                _text.text = string.Empty;
                InputManager.Instance.PassDialogue -= PassDials;
                _buttonsToPressText.text = string.Empty;
            }
        }
    }

    private void PassDials()
    {      
        _text.text = string.Empty;
        _buttonsToPressText.text = string.Empty;
        _timeStamp = 0;
        _source.Stop();
    }

    public void ClearAll()
    {
        PassDials();
        if (_voiceDialBoxDataList != null)
            _voiceDialBoxDataList.Clear();
        _voiceDialBoxDataList = null;
        if(_textDialBoxDataList != null)
            _textDialBoxDataList.Clear();
        _textDialBoxDataList = null;
        _timerIsStarted = false;
    }
}