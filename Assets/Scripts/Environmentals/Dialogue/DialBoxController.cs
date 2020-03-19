using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialBoxController : MonoBehaviour
{
    [SerializeField ,Header("TmPro Text Zone")] private TextMeshProUGUI _text = null;
    [SerializeField ,Header("Blank Time Between Text")] private float _blankThreshold = 0.1f;

    private AudioSource _source = null;
    private float _timeStamp = 0.0f;
    private List<DialBoxData> _dialBoxDataList = null;
    private bool _timerIsStarted = false;

    private void Start()
    {
        _source = PlayerManager.Instance.Player.AudioSource;
        NarrativeManager.Instance.OnTriggerNarrative += OnTriggerNarrative;
    }

    private void OnTriggerNarrative(DialBoxData[] dialBoxs)
    {
        _dialBoxDataList = new List<DialBoxData>(dialBoxs);
        TriggerFirstElementDialBox();
    }

    private void TriggerFirstElementDialBox()
    {
        _text.text = _dialBoxDataList[0].Text;
        _source.PlayOneShot(_dialBoxDataList[0].Clip);
        _timeStamp = Time.time + (_dialBoxDataList[0].Clip.length + _dialBoxDataList[0].LifeTime);
        _timerIsStarted = true;
        _dialBoxDataList.RemoveAt(0);
    }

    private void Update()
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
            }
        }
    }
}
