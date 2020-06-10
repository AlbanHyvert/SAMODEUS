using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Scale : MonoBehaviour
{
    [SerializeField] private string _objectTag = "Diamond";
    [SerializeField] private Transform _setPosition = null;
    [SerializeField] private GameObject[] _activatedObject = null;
    [SerializeField] private GameObject[] _desactivedObject = null;
    [Space]
    [SerializeField] private DialogueSystem _dialogues = null;

    private void Start()
    {
        NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

        for (int i = 0; i < _dialogues.TextID.Length; i++)
        {
            string newID = _dialogues.TextID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
            _dialogues.TextID[i] = newID;
        }

        if (_activatedObject != null)
        {
            for (int i = 0; i < _activatedObject.Length; i++)
            {
                _activatedObject[i].SetActive(false);
            }
        }

        if (_desactivedObject != null)
        {
            for (int i = 0; i < _desactivedObject.Length; i++)
            {
                _desactivedObject[i].SetActive(true);
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

        if(action != null && other.tag == _objectTag)
        {
            PlayerManager.Instance.Player.GetComponent<PlayerController>().OnDrop();
            action.DestroySelf(_setPosition);

            if (_dialogues != null)
            {
                List<DialogueSystem> dialogueSystems = new List<DialogueSystem>();

                dialogueSystems.Add(_dialogues);

                NarrativeManager.Instance.TriggerNarrative(dialogueSystems.ToArray());
            }

            if (_activatedObject != null)
            {
                for (int i = 0; i < _activatedObject.Length; i++)
                {
                    _activatedObject[i].SetActive(true);
                }
            }

            if (_desactivedObject != null)
            {
                for (int i = 0; i < _desactivedObject.Length; i++)
                {
                    _desactivedObject[i].SetActive(false);
                }
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
