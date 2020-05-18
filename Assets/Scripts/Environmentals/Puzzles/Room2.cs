using UnityEditor;
using UnityEngine;

public class Room2 : MonoBehaviour
{
    [SerializeField, Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField, Header("Voice Boxs ID")] private string[] _voiceBoxID = null;
    [SerializeField] private bool _shouldStopPlayer = false;
    [SerializeField] private float _stopPlayerDuration = 5;
    [SerializeField] private float _timeBeforeSpawnObj = 5;
    [SerializeField] private GameObject _wrongObjToSpawn = null;
    [SerializeField] private GameObject _goodObjToSpawn = null;
    [SerializeField] private Transform _objSpawnPosition = null;

    private bool _stopPlayer = false;
    private float _stopPlayerTimer = 0;
    private float _spawnObjTimer = 0;
    private bool _shouldSpawnObj = false;
    private GameObject _objToDestroy = null;

    public GameObject ObjectToDestroy { get { return _objToDestroy; } set { _objToDestroy = value; } }

    private void Start()
    {
        NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

        if (_textBoxID != null)
        {
            for (int i = 0; i < _textBoxID.Length; i++)
            {
                if (_textBoxID[i] != string.Empty)
                {
                    string newID = _textBoxID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                    _textBoxID[i] = newID;
                }
            }
        }
    }

    private void OnLanguageChange(GameManager.Language language)
    {
        if(_textBoxID != null)
        {
            for (int i = 0; i < _textBoxID.Length; i++)
            {
                if (_textBoxID[i] != string.Empty)
                {
                    string newID = _textBoxID[i] + "_" + language.ToString();
                    _textBoxID[i] = newID;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(_textBoxID.Length > 1)
            {
                if (NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
                {
                    NarrativeManager.Instance.TriggerNarrative(_textBoxID, _voiceBoxID);
                    
                }
                else
                {
                    NarrativeManager.Instance.DialBoxController.ClearAll();
                    NarrativeManager.Instance.TriggerNarrative(_textBoxID, _voiceBoxID);
                }
            }

            _spawnObjTimer = Time.time + _timeBeforeSpawnObj;
            _shouldSpawnObj = true;
            GameLoopManager.Instance.Puzzles += OnUpdate;

            if (_shouldStopPlayer == true)
            {
                _stopPlayer = true;
                _stopPlayerTimer = Time.time + _stopPlayerDuration;
            }

            gameObject.SetActive(false);
        }
    }

    private void OnUpdate()
    {
        if (_stopPlayer == false && _shouldSpawnObj == false)
        {
            GameLoopManager.Instance.Puzzles -= OnUpdate;
        }

        if (_stopPlayer == true)
        {
            if (Time.time <= _stopPlayerTimer)
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
            _stopPlayerTimer = 0;
        }

        if(_shouldSpawnObj == true && Time.time >= _spawnObjTimer)
        {
            if(_wrongObjToSpawn != null)
            {
                _objToDestroy = Instantiate(_wrongObjToSpawn, _objSpawnPosition.position, Quaternion.identity);
            }

            _shouldSpawnObj = false;
        }
    }
}