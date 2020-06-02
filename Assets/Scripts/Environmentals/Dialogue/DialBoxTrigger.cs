using UnityEngine;
using TMPro;

public class DialBoxTrigger : MonoBehaviour
{
    [SerializeField ,Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField ,Header("Voice Boxs ID")] private string[] _voiceBoxID = null;
    [SerializeField] private GameObject[] _activateNextObj = null;
    [SerializeField] private bool _shouldStopPlayer = false;
    [SerializeField] private float _stopPlayerDuration = 5;

    private bool _stopPlayer = false;
    private float _timer = 0;

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

            if(_shouldStopPlayer == true)
            {
                GameLoopManager.Instance.Puzzles += OnUpdate;
                _stopPlayer = true;
                _timer = Time.time + _stopPlayerDuration;
            }

            if(_activateNextObj != null)
            {
                for (int i = 0; i < _activateNextObj.Length; i++)
                {
                    if(_activateNextObj[i] != null)
                    {
                        _activateNextObj[i].SetActive(true);
                    }
                }
            }
        }
    }

    private void OnUpdate()
    {
        if(_stopPlayer == true)
        {
            if (Time.time <= _timer)
            {
                PlayerManager.Instance.Player.MovementShouldStop(_shouldStopPlayer);
            }
            else
            {
                _stopPlayer = false;
            }
        }
        else
        {
            PlayerManager.Instance.Player.MovementShouldStop(false);
            _timer = 0;
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
    }
}
