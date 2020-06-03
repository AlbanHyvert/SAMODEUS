using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialBoxController : MonoBehaviour
{
    [SerializeField ,Header("TmPro Text Zone")] private TextMeshProUGUI _text = null;
    [SerializeField ,Header("Blank Time Between Text")] private float _blankThreshold = 0.1f;
    [SerializeField] private TextMeshProUGUI _buttonsToPressText = null;

    private AudioSource _source = null;
    private float _textTimeStamp = 0.0f;
    private float _voiceTimeStamp = 0.0f;
    private List<TextDialBoxData> _textDialBoxDataList = null;
    private List<VoiceDialBoxData> _voiceDialBoxDataList = null;
    private bool _timerIsStarted = false;

    public bool TimerIsStarted { get { return _timerIsStarted; } }

    public AudioSource AudioSource { get { return _source; } set { _source = value; } }

    private void Awake()
    {
        NarrativeManager.Instance.DialBoxController = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (_buttonsToPressText != null)
            _buttonsToPressText.text = string.Empty;

        _text.text = string.Empty;

        if(PlayerManager.Instance.Player != null)
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
        _source.PlayOneShot(_voiceDialBoxDataList[0].Clip);
        _voiceTimeStamp = Time.time + (_voiceDialBoxDataList[0].Clip.length + _voiceDialBoxDataList[0].LifeTime);
        _voiceDialBoxDataList.RemoveAt(0);
    }

    private void TriggerFirstTextDialsElement()
    {
        if (_text.text == string.Empty)
        {
            _text.text = _textDialBoxDataList[0].Text;
            _textTimeStamp = Time.time + _textDialBoxDataList[0].LifeTime;
            _timerIsStarted = true;
            _textDialBoxDataList.RemoveAt(0);
        }
    }

    private void OnUpdate()
    {
        if(_source != null && _timerIsStarted == true)
        {
            _buttonsToPressText.text = InputManager.Instance.DataKeycode.KeyDialogue.ToString();

            if (Time.time >= _textTimeStamp)
            {
                if(_textDialBoxDataList != null && _textDialBoxDataList.Count > 0)
                {
                    TriggerFirstTextDialsElement();
                }
                else
                {
                    _textDialBoxDataList = null;
                    _text.text = string.Empty;
                }
            }
            else if(Time.time >= _textTimeStamp - _blankThreshold)
            {
                _text.text = string.Empty;
            }

            if(Time.time >= _voiceTimeStamp)
            {
                if (_voiceDialBoxDataList != null && _voiceDialBoxDataList.Count > 0)
                {
                    TriggerFirstAudioDialsElement();
                }
                else
                {
                    _voiceDialBoxDataList = null;
                }
            }

            if (_voiceDialBoxDataList == null && _textDialBoxDataList == null && _source.isPlaying == false)
            {
                _source.clip = null;
                _buttonsToPressText.text = string.Empty;
                _text.text = string.Empty;
                InputManager.Instance.PassDialogue -= PassDials;
                _timerIsStarted = false;
            }
        }
        else
        {
            _buttonsToPressText.text = string.Empty;
            InputManager.Instance.PassDialogue -= PassDials;
        }
    }

    private void PassDials()
    {      
        _text.text = string.Empty;
        _buttonsToPressText.text = string.Empty;
        _textTimeStamp = 0;
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
        _text.text = string.Empty;
        _timerIsStarted = false;
    }

    public void OnDestroy()
    {
        if(NarrativeManager.Instance != null)
            NarrativeManager.Instance.OnCallNarration -= OnTriggerNarration;

        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Pause -= IsPause;
            GameLoopManager.Instance.UI -= OnUpdate;
        }
    }
}