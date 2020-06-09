using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private DialogueSystem _dialogues = null;
    [Space]
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private Transform[] _bridgeParts = null;
    [SerializeField] private Collider[] _colliders = null;

    private List<RotatingCube> _activatedBridge = null;

    private void Start()
    {
        NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

        for (int i = 0; i < _dialogues.TextID.Length; i++)
        {
            string newID = _dialogues.TextID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
            _dialogues.TextID[i] = newID;
        }

        _activatedBridge = new List<RotatingCube>();

        for (int i = 0; i < _bridgeParts.Length; i++)
        {
            foreach (Transform item in _bridgeParts[i])
            {
                RotatingCube cube = item.GetComponent<RotatingCube>();

                if (cube != null)
                {
                    _activatedBridge.Add(cube);
                }
            }
        }

        if (_colliders != null)
        {
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }
        }
    }

    private void OnLanguageChange(GameManager.Language language)
    {
        for (int i = 0; i < _dialogues.TextID.Length; i++)
        {
            string newID = _dialogues.TextID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
            _dialogues.TextID[i] = newID;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IAction action = other.GetComponent<IAction>();

        if (action != null && other.tag == _objectTag)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerController>().OnDrop();
            action.DestroySelf(_setPosition);

            if(_dialogues != null)
            {
                List<DialogueSystem> dialogueSystems = new List<DialogueSystem>();

                dialogueSystems.Add(_dialogues);

                NarrativeManager.Instance.TriggerNarrative(dialogueSystems.ToArray());
            }

            if(_colliders != null)
            {
                for (int i = 0; i < _colliders.Length; i++)
                {
                    _colliders[i].enabled = true;
                }
            }

            if (_activatedBridge != null)
            {
                for (int i = 0; i < _activatedBridge.Count; i++)
                {
                    _activatedBridge[i].SetOverrideState(true);
                }
            }
        }
    }
}