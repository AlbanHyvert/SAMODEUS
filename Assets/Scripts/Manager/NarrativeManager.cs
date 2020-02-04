using Engine.Singleton;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NarrativeManager : Singleton<NarrativeManager>
{
    [SerializeField, Header("Dials Boxs Data")] private DialBoxData[] _dialBoxData = null;
    private Dictionary<string, DialBoxData> _dialBoxs = null;

    private event Action<DialBoxData[]> _onTriggerNarrative = null;
    public event Action<DialBoxData[]> OnTriggerNarrative
    {
        add
        {
            _onTriggerNarrative -= value;
            _onTriggerNarrative += value;
        }
        remove
        {
            _onTriggerNarrative -= value;
        }
    }

    private void Start()
    {
        _dialBoxs = new Dictionary<string, DialBoxData>();
        for (int i = 0; i < _dialBoxData.Length; i++)
        {
            _dialBoxs.Add(_dialBoxData[i].ID, _dialBoxData[i]);
        }
    }

    public void TriggerNarrative(string[] ID)
    {
        if (_onTriggerNarrative != null)
        {
            List<DialBoxData> dialBoxDatas = new List<DialBoxData>();

            for (int i = 0; i < ID.Length; i++)
            {
                dialBoxDatas.Add(_dialBoxs[ID[i]]);
            }
            _onTriggerNarrative(dialBoxDatas.ToArray());
        }
    }
}
