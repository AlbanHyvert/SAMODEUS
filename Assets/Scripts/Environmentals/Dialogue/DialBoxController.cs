using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialBoxController : MonoBehaviour
{
    [SerializeField ,Header("TmPro Text Zone")] private TextMeshProUGUI _text = null;
    [SerializeField ,Header("Blank Time Between Text")] private float _blankThreshold = 0.1f;
    [SerializeField] private TextMeshProUGUI _buttonsToPressText = null;

    private AudioSource _source = null;
    private float _timeStamp = 0.0f;
    private List<DialBoxData> _dialBoxDataList = null;
    private bool _timerIsStarted = false;

    private void Start()
    {
        if (_buttonsToPressText != null)
            _buttonsToPressText.text = string.Empty;

        _text.text = string.Empty;
        _source = PlayerManager.Instance.Player.DialsAudioSource;
        NarrativeManager.Instance.OnTriggerNarrative += OnTriggerNarrative;
        GameLoopManager.Instance.Pause += IsPause;
        GameLoopManager.Instance.UI += OnUpdate;
        InputManager.Instance.PassDialogue += PassDials;
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

    private void OnTriggerNarrative(DialBoxData[] dialBoxs)
    {
        _dialBoxDataList = new List<DialBoxData>(dialBoxs);
        TriggerFirstElementDialBox();
    }

    private void TriggerFirstElementDialBox()
    {
        if (_text.text == string.Empty)
        {
            _text.text = _dialBoxDataList[0].Text;
            _source.PlayOneShot(_dialBoxDataList[0].Clip);
            _timeStamp = Time.time + (_dialBoxDataList[0].Clip.length + _dialBoxDataList[0].LifeTime);
            _timerIsStarted = true;
            _dialBoxDataList.RemoveAt(0);
            _buttonsToPressText.text = InputManager.Instance.DataKeycode.KeyDialogue.ToString();
        }
    }

    private void OnUpdate()
    {
        if(_timerIsStarted == true)
        {
            if(Time.time >= _timeStamp)
            {
                if (_dialBoxDataList.Count > 0)
                {
                    TriggerFirstElementDialBox();
                }
                else
                {
                    _dialBoxDataList = null;
                    _timerIsStarted = false;
                }
            }
            else if(Time.time >= _timeStamp - _blankThreshold)
            {
                _text.text = string.Empty;
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
}
