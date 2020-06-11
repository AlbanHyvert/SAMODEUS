using UnityEngine;

public class Room2 : MonoBehaviour
{
    [Header("First Dial Event")]
    [SerializeField] private string[] _firstTextBoxID = null;
    [SerializeField] private string[] _firstVoiceBoxID = null;

    [Header("Second Dial Event")]
    [SerializeField] private string[] _secondTextBoxID = null;
    [SerializeField] private string[] _secondVoiceBoxID = null;

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

        if (_firstTextBoxID != null)
        {
            for (int i = 0; i < _firstTextBoxID.Length; i++)
            {
                if (_firstTextBoxID[i] != string.Empty)
                {
                    string newID = _firstTextBoxID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                    _firstTextBoxID[i] = newID;
                }
            }
        }

        if (_secondTextBoxID != null)
        {
            for (int i = 0; i < _secondTextBoxID.Length; i++)
            {
                if (_secondTextBoxID[i] != string.Empty)
                {
                    string newID = _secondTextBoxID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                    _secondTextBoxID[i] = newID;
                }
            }
        }
    }

    private void OnLanguageChange(GameManager.Language language)
    {
        if(_firstTextBoxID != null)
        {
            for (int i = 0; i < _firstTextBoxID.Length; i++)
            {
                if (_firstTextBoxID[i] != string.Empty)
                {
                    string newID = _firstTextBoxID[i] + "_" + language.ToString();
                    _firstTextBoxID[i] = newID;
                }
            }
        }

        if (_secondTextBoxID != null)
        {
            for (int i = 0; i < _secondTextBoxID.Length; i++)
            {
                if (_secondTextBoxID[i] != string.Empty)
                {
                    string newID = _secondTextBoxID[i] + "_" + language.ToString();
                    _secondTextBoxID[i] = newID;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(_firstTextBoxID.Length >= 1)
            {
                if (NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
                {
                    NarrativeManager.Instance.TriggerNarrative(_firstTextBoxID, _firstVoiceBoxID);
                    
                }
                else
                {
                    NarrativeManager.Instance.DialBoxController.ClearAll();
                    NarrativeManager.Instance.TriggerNarrative(_firstTextBoxID, _firstVoiceBoxID);
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

            Collider collider = gameObject.GetComponent<Collider>();

            collider.enabled = false;
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
            _stopPlayerTimer = 0;
        }

        if(_shouldSpawnObj == true && Time.time >= _spawnObjTimer)
        {
            if(_wrongObjToSpawn != null)
            {
                _objToDestroy = Instantiate(_wrongObjToSpawn, _objSpawnPosition.position, Quaternion.identity);
            }
            else
            {
                _objToDestroy = Instantiate(_goodObjToSpawn, _objSpawnPosition.position, Quaternion.identity);
            }

            _wrongObjToSpawn = null;
            _shouldSpawnObj = false;
        }
    }

    public void StartSecondEvent()
    {
        if (_firstTextBoxID.Length > 1)
        {
            if (NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
            {
                NarrativeManager.Instance.TriggerNarrative(_secondTextBoxID, _secondVoiceBoxID);
            }
            else
            {
                NarrativeManager.Instance.DialBoxController.ClearAll();
                NarrativeManager.Instance.TriggerNarrative(_secondTextBoxID, _secondVoiceBoxID);
            }
        }

        _spawnObjTimer = Time.time + _timeBeforeSpawnObj;
        _shouldSpawnObj = true;

        Destroy(_objToDestroy);
        _objToDestroy = null;

        GameLoopManager.Instance.Puzzles += OnUpdate;
    }
}