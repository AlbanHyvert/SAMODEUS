using UnityEngine;

public class DialBoxTrigger : MonoBehaviour
{
    [SerializeField ,Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField ,Header("Voice Boxs ID")] private string[] _voiceBoxID = null;
    [Space]
    [SerializeField] private GameObject[] _activateNextObj = null;
    [SerializeField] private GameObject[] _desactivateNextObj = null;
    [Space]
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

    private void OnTriggerStay(Collider other)
    {
        if(NarrativeManager.Instance.DialBoxController.AudioSource != null)
        {
            if (other.tag.Equals("Player"))
            {
                if (NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
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

                if (_shouldStopPlayer == true)
                {
                    GameLoopManager.Instance.Puzzles += OnUpdate;
                    _stopPlayer = true;
                    _timer = Time.time + _stopPlayerDuration;
                }

                if (_activateNextObj != null)
                {
                    for (int i = 0; i < _activateNextObj.Length; i++)
                    {
                        if (_activateNextObj[i] != null)
                        {
                            _activateNextObj[i].SetActive(true);
                        }
                    }
                }

                if (_desactivateNextObj != null)
                {
                    for (int i = 0; i < _desactivateNextObj.Length; i++)
                    {
                        if (_desactivateNextObj[i] != null)
                        {
                            _desactivateNextObj[i].SetActive(false);
                        }
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
                PlayerManager.Instance.PlayerCanMove = !_shouldStopPlayer;
            }
            else
            {
                _stopPlayer = false;
            }
        }
        else
        {
            PlayerManager.Instance.PlayerCanMove = true;
            _timer = 0;
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
    }

    private void OnDestroy()
    {
        if(NarrativeManager.Instance != null)
        {
            NarrativeManager.Instance.ChangeLanguages -= OnLanguageChange;
        }

        if(GameLoopManager.Instance != null)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }
    }
}