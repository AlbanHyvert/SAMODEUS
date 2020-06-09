using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DeathZone : MonoBehaviour
{
    [SerializeField] private DialogueSystem[] _dialogues = null;
    [Space]
    [SerializeField] private bool _isRandom = true;

    private List<DialogueSystem> _dialogueList = null;
    private int _i = 0;

    private void Start()
    {
        _dialogueList = new List<DialogueSystem>();

        _i = 0;

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

    private void OnLanguageChange(GameManager.Language language)
    {
        for (int i = 0; i < _dialogues.Length; i++)
        {
            for (int j = 0; j < _dialogues[i].TextID.Length; j++)
            {
                string newID = _dialogues[i].TextID[j] + "_" + language.ToString();
                _dialogues[i].TextID[j] = newID;
            }
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
            if(_dialogues != null)
            {
                if(_i < _dialogues.Length)
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