using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RoomsDoor : MonoBehaviour
{
    [SerializeField] private DialogueSystem[] _dialogues = null;
    [Space]
    [SerializeField] private GameObject[] _door = null;
    [SerializeField] private int _roomStatus = 1;

    private List<bool> _isActive = null;
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
            if (_dialogues != null && _dialogues.Length > 1)
            {
                if (InfiniteRoomManager.Instance.DialRoom1 < _dialogues.Length)
                {
                    _dialogueList.Add(_dialogues[InfiniteRoomManager.Instance.DialRoom1]);
                    InfiniteRoomManager.Instance.DialRoom1++;
                }
                else
                {
                    InfiniteRoomManager.Instance.DialRoom1 = 0;
                    _dialogueList.Add(_dialogues[InfiniteRoomManager.Instance.DialRoom1]);
                }

                NarrativeManager.Instance.TriggerNarrative(_dialogueList.ToArray());

                _dialogueList.Clear();
            }

            if (_door != null)
            {
                for (int j = 0; j < _door.Length; j++)
                {
                    _door[j].SetActive(!_isActive[j]);
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
