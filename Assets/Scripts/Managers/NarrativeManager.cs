using Engine.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : Singleton<NarrativeManager>
{
    [SerializeField, Header("Dials Boxs Data")] private DialBoxData[] _dialBoxData = null;
    [SerializeField, Header("Text Dials Boxs Data")] private TextDialBoxData[] _textDialBoxData = null;
    [SerializeField, Header("Voice Dials Boxs Data")] private VoiceDialBoxData[] _voiceDialBoxData = null;
    
    private DialBoxController _dialBoxController = null;

    private Dictionary<string, DialBoxData> _dialBoxs = null;
    private Dictionary<string, TextDialBoxData> _textDialBoxs = null;
    private Dictionary<string, VoiceDialBoxData> _voiceDialBoxs = null;

    private event Action<VoiceDialBoxData[], TextDialBoxData[]> _onCallNarration = null;
    public event Action<VoiceDialBoxData[], TextDialBoxData[]> OnCallNarration
    {
        add
        {
            _onCallNarration -= value;
            _onCallNarration += value;
        }
        remove
        {
            _onCallNarration -= value;
        }
    }

    public DialBoxController DialBoxController { get { return _dialBoxController; } set { _dialBoxController = value; } }

    private void Start()
    {
        _textDialBoxs = new Dictionary<string, TextDialBoxData>();
        _voiceDialBoxs = new Dictionary<string, VoiceDialBoxData>();

        for (int i = 0; i < _textDialBoxData.Length; i++)
        {
            _textDialBoxs.Add(_textDialBoxData[i].ID, _textDialBoxData[i]);
        }

        for (int i = 0; i < _voiceDialBoxData.Length; i++)
        {
            _voiceDialBoxs.Add(_voiceDialBoxData[i].ID, _voiceDialBoxData[i]);
        }
    }

    public void TriggerNarrative(string[] ID)
    {
        if(_onCallNarration != null)
        {
            List<VoiceDialBoxData> voiceDialBoxData = new List<VoiceDialBoxData>();
            List<TextDialBoxData> textDialBoxDatas = new List<TextDialBoxData>();

            for (int i = 0; i < ID.Length; i++)
            {
                voiceDialBoxData.Add(_voiceDialBoxs[ID[i]]);
                textDialBoxDatas.Add(_textDialBoxs[ID[i]]);
            }

            Debug.Log("Index Size : " + _voiceDialBoxs.Count);
            Debug.Log("Index Size : " + _textDialBoxs.Count);
            Debug.Log("Index Temp Size : " + voiceDialBoxData.Count);
            Debug.Log("Index Temp Size : " + textDialBoxDatas.Count);

            _onCallNarration(voiceDialBoxData.ToArray(), textDialBoxDatas.ToArray());
        }
    }
}
