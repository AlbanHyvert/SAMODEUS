using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomsDoor : MonoBehaviour
{
    [SerializeField] private DialogueSystem[] _dialogues = null;
    [Space]
    [SerializeField] private GameObject[] _door = null;

    private List<bool> _isActive = null;

    private int _i = 0;
    private List<DialogueSystem> _dialogueList = null;

    private void Start()
    {
        _dialogueList = new List<DialogueSystem>();

        if(_dialogues != null)
        {
            NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

            for (int i = 0; i < _dialogues.Length; i++)
            {
                for (int j = 0; j < _dialogues[i].TextID.Length; j++)
                {
                    string newID = _dialogues[i].TextID[j] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                    _dialogues[i].TextID[j] = newID;
                }
            }
        }

        _isActive = new List<bool>();

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        rigidbody.isKinematic = true;

        if (_door != null)
        {
            for (int i = 0; i < _door.Length; i++)
            {
                _isActive.Add(_door[i].activeSelf);
            }
        }
    }

    private void OnLanguageChange(GameManager.Language language)
    {
        for (int i = 0; i < _dialogues.Length; i++)
        {
            for (int j = 0; j < _dialogues[i].TextID.Length; j++)
            {
                string newID = _dialogues[i].TextID[j] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                _dialogues[i].TextID[j] = newID;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            if (_dialogues != null)
            {
                if (_i < _dialogues.Length)
                {
                    _dialogueList.Add(_dialogues[_i]);
                    _i++;
                }
                else
                {
                    _i = 0;
                    _dialogueList.Add(_dialogues[_i]);
                }

                NarrativeManager.Instance.TriggerNarrative(_dialogueList.ToArray());

                _dialogueList.Clear();
            }

            if (_door != null)
            {
                for (int i = 0; i < _door.Length; i++)
                {
                    _door[i].SetActive(!_isActive[i]);
                }
            }
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            _isActive.Clear();

            if (_door != null)
            {
                for (int i = 0; i < _door.Length; i++)
                {
                    _isActive.Add(_door[i].activeSelf);
                }
            }
        }
    }
}
