using Engine.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField ,Header("Sound Boxs Data")] private SoundBoxData[] _soundBoxData = null;
    private Dictionary<string, SoundBoxData> _soundBoxs = null;

    private event Action<SoundBoxData> _onTriggerSoundUI = null;
    public event Action<SoundBoxData> OnTriggerSoundUI
    {
        add
        {
            _onTriggerSoundUI -= value;
            _onTriggerSoundUI += value;
        }
        remove
        {
            _onTriggerSoundUI -= value;
        }
    }

    private void Start()
    {
        _soundBoxs = new Dictionary<string, SoundBoxData>();
        for (int i = 0; i < _soundBoxData.Length; i++)
        {
            _soundBoxs.Add(_soundBoxData[i].ID, _soundBoxData[i]);
        }
    }

    public void TriggerSound(string ID)
    {
        if(_onTriggerSoundUI != null)
        {
            _onTriggerSoundUI(_soundBoxs[ID]);
        }
    }
}
