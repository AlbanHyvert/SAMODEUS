using UnityEngine;

[System.Serializable]
public class DialogueSystem
{
    [SerializeField] private string[] _textID = null;
    [SerializeField] private string _voiceID = string.Empty;

    public string[] TextID { get { return _textID; } }
    public string VoiceID { get { return _voiceID; } }
}