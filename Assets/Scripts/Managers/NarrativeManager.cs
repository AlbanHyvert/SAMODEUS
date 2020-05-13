using Engine.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : Singleton<NarrativeManager>
{
    [SerializeField, Header("Text Dials Boxs Data")] private TextDialBoxData[] _textDialBoxData = null;
    [SerializeField, Header("Voice Dials Boxs Data")] private VoiceDialBoxData[] _voiceDialBoxData = null;
    [SerializeField] private GameManager.Language _choosenLanguage = GameManager.Language.FRENCH;

    private DialBoxController _dialBoxController = null;
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

    private event Action<GameManager.Language> _changeLanguages = null;
    public event Action<GameManager.Language> ChangeLanguages
    {
        add
        {
            _changeLanguages -= value;
            _changeLanguages += value;
        }
        remove
        {
            _changeLanguages -= value;
        }
    }

    public DialBoxController DialBoxController { get { return _dialBoxController; } set { _dialBoxController = value; } }
    public GameManager.Language ChoosenLanguage { get { return _choosenLanguage; } 
        set
        {
            _choosenLanguage = value;
            _changeLanguages(_choosenLanguage);
        }
    }

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

    public void TriggerNarrative(string[] textID, string[] voiceID)
    {
        if(_onCallNarration != null)
        {
            List<VoiceDialBoxData> voiceDialBoxData = new List<VoiceDialBoxData>();
            List<TextDialBoxData> textDialBoxDatas = new List<TextDialBoxData>();

            for (int i = 0; i < textID.Length; i++)
            {
                textDialBoxDatas.Add(_textDialBoxs[textID[i]]);
            }

            for (int i = 0; i < voiceID.Length; i++)
            {
                voiceDialBoxData.Add(_voiceDialBoxs[voiceID[i]]);
            }

            _onCallNarration(voiceDialBoxData.ToArray(), textDialBoxDatas.ToArray());
        }
    }
}
