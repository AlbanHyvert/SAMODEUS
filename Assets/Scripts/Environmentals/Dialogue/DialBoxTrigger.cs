using UnityEngine;
using TMPro;

public class DialBoxTrigger : MonoBehaviour
{
    [SerializeField ,Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField ,Header("Voice Boxs ID")] private string[] _voiceBoxID = null;

    private void Start()
    {
        NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

        for (int i = 0; i < _textBoxID.Length; i++)
        {
            string newID = _textBoxID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
            _textBoxID[i] = newID;
        }
    }

    private void OnLanguageChange(GameManager.Language language)
    {
        for (int i = 0; i < _textBoxID.Length; i++)
        {
            string newID = _textBoxID[i] + "_" + language.ToString();
            _textBoxID[i] = newID;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
            {
                NarrativeManager.Instance.TriggerNarrative(_textBoxID, _voiceBoxID);
                gameObject.SetActive(false);
            }
            else
            {
                NarrativeManager.Instance.DialBoxController.ClearAll();
                NarrativeManager.Instance.TriggerNarrative(_textBoxID, _voiceBoxID);
                gameObject.SetActive(false);
            }
        }
    }
}
