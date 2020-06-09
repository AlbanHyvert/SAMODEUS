using System.Collections.Generic;
using UnityEngine;

public class ResetTexture : MonoBehaviour
{
    [SerializeField] private Material _material = null;
    [SerializeField] private bool _isPortal = false;
    [SerializeField] private Room2 _room2 = null;
    [Space]
    [SerializeField] private DialogueSystem _dials = null;

    private void Start()
    {
        if(_dials != null)
        {
            NarrativeManager.Instance.ChangeLanguages += OnLanguageChange;

            for (int i = 0; i < _dials.TextID.Length; i++)
            {
                string newID = _dials.TextID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
                _dials.TextID[i] = newID;
            }
        }

    }

    private void OnLanguageChange(GameManager.Language language)
    {
        for (int i = 0; i < _dials.TextID.Length; i++)
        {
            string newID = _dials.TextID[i] + "_" + NarrativeManager.Instance.ChoosenLanguage.ToString();
            _dials.TextID[i] = newID;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickable pickable = other.GetComponent<Pickable>();

        if(pickable != null)
        {
            if (pickable.gameObject.tag.Equals("WrongObj"))
            {
                _room2.StartSecondEvent();
            }
            else
            {
                if (pickable.HasAlreadyStarted == false)
                {
                    if(_dials != null)
                    {
                        List<DialogueSystem> dialogueSystems = new List<DialogueSystem>();

                        dialogueSystems.Add(_dials);

                        NarrativeManager.Instance.TriggerNarrative(dialogueSystems.ToArray());
                    }

                    pickable.HasAlreadyStarted = true;
                }

                if (_isPortal == false && pickable != null)
                {
                    GetComponent<Renderer>().material = _material;
                }
                else if (_isPortal == true && pickable != null)
                {
                    GetComponent<Renderer>().material = _material;
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}