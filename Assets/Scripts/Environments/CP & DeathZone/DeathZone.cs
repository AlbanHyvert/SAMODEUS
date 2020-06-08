using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeathZone : MonoBehaviour
{
    [SerializeField, Header("Text Boxs ID")] private string[] _textBoxID = null;
    [SerializeField, Header("Voice Boxs ID")] private string[] _voiceBoxID = null;
    [Space]
    [SerializeField] private bool _isRandom = true;

    private List<string> _tempArrayText = null;
    private List<string> _tempArrayDial = null;
    private int _iText = 0;
    private int _iDial = 0;

    private void Start()
    {
        _tempArrayDial = new List<string>();
        _tempArrayText = new List<string>();

        _iDial = 0;
        _iText = 0;

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

    private void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterController player = other.transform.GetComponent<CharacterController>();
        Pickable pickable = other.transform.GetComponent<Pickable>();

        if(player != null)
        {
            if(_textBoxID.Length > 1 && _voiceBoxID.Length > 1)
            {
                if (_isRandom == true)
                {
                    int Itext = Random.Range(0, _textBoxID.Length - 1);
                    int IDial = Random.Range(0, _voiceBoxID.Length - 1);

                    _tempArrayDial.Add(_voiceBoxID[IDial]);
                    _tempArrayText.Add(_textBoxID[Itext]);
                }
                else
                {
                    if (_iText < _textBoxID.Length)
                    {
                        _tempArrayText.Add(_textBoxID[_iText]);
                        _iText++;
                    }
                    else
                    {
                        _iText = 0;
                        _tempArrayText.Add(_textBoxID[_iText]);
                    }

                    if (_iDial < _voiceBoxID.Length)
                    {
                        _tempArrayDial.Add(_tempArrayDial[_iDial]);
                        _iDial++;
                    }
                    else
                    {
                        _iDial = 0;
                        _tempArrayDial.Add(_tempArrayDial[_iDial]);
                    }
                }

                if (_tempArrayDial.Count != 0 && _tempArrayText.Count != 0)
                {
                    if (NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
                    {
                        NarrativeManager.Instance.TriggerNarrative(_tempArrayText.ToArray(), _tempArrayDial.ToArray());
                    }
                    else
                    {
                        NarrativeManager.Instance.DialBoxController.ClearAll();
                        NarrativeManager.Instance.TriggerNarrative(_tempArrayText.ToArray(), _tempArrayDial.ToArray());
                    }
                }

                _tempArrayText.Clear();
                _tempArrayDial.Clear();
            }

            PlayerManager.Instance.UseGravity = false;
            RespawnManager.Instance.Respawn(player);
        }

        if(pickable != null)
        {
            if(pickable.ShouldBeDestroyed == true)
            {
                Destroy(pickable.gameObject);
            }
            else
            {
                RespawnManager.Instance.ResetObjectPosition(pickable);
            }
        }
    }

    private void OnDestroy()
    {
        if(NarrativeManager.Instance != null)
        {
            NarrativeManager.Instance.ChangeLanguages -= OnLanguageChange;
        }
    }
}