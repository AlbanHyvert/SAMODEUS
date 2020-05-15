using UnityEngine;

public class Room4 : MonoBehaviour
{
    [SerializeField, Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField, Header("Voice Boxs ID")] private string[] _voiceBoxID = null;
    [Header("Player Event")]
    [SerializeField] private bool _shouldStopPlayer = false;
    [SerializeField] private float _stopPlayerDuration = 5;
    [Header("Room 4 Event")]
    [SerializeField] private Transform _objectPosToSpawn = null;
    [SerializeField] private GameObject _objectToSpawn = null;
    [SerializeField] private Pickable _objectToInteract = null;
    [SerializeField] private float _timerBeforeSecondEvent = 5;

    private bool _stopPlayer = false;
    private float _dialTimer = 0;
    private float _eventTimer = 0;
    private bool _activateEvent = false;
    private GameObject _tempObj = null;

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
        GameLoopManager.Instance.Puzzles += OnUpdate;

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
        }

        if (_shouldStopPlayer == true)
        {
            _stopPlayer = true;
            _dialTimer = Time.time + _stopPlayerDuration;
        }
    }

    private void OnUpdate()
    {
        if (Time.time <= _dialTimer)
        {
            if (_stopPlayer == true)
            {
                PlayerManager.Instance.Player.MovementShouldStop(_shouldStopPlayer);
                _tempObj = Instantiate(_objectToSpawn, _objectPosToSpawn.position, Quaternion.identity);
                _stopPlayer = false;
            }
        }
        else
        {
            PlayerManager.Instance.Player.MovementShouldStop(false);
            _dialTimer = 0;
            _eventTimer = Time.time + _timerBeforeSecondEvent;
            _activateEvent = true;
        }

        if (_activateEvent == true && Time.time >= _eventTimer)
        {
            Object.Destroy(_tempObj);
            _tempObj = null;
            _tempObj = Instantiate(_objectToInteract.gameObject, _objectPosToSpawn.position, Quaternion.identity);
            _activateEvent = false;
            _eventTimer = 0;
        }
    }
}