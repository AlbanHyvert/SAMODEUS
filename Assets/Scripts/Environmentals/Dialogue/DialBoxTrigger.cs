using UnityEngine;
using TMPro;

public class DialBoxTrigger : MonoBehaviour
{
    [SerializeField ,Header("Dials Boxs ID")] private string[] _dialBoxID = null;

    private void Start()
    {
        for (int i = 0; i < _dialBoxID.Length; i++)
        {
            string newID = _dialBoxID[i] + "_" + GameManager.Instance.Languages.ToString();
            _dialBoxID[i] = newID;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            if(NarrativeManager.Instance.DialBoxController.TimerIsStarted == false)
            {
                NarrativeManager.Instance.TriggerNarrative(_dialBoxID);
                gameObject.SetActive(false);
            }
            else
            {
                NarrativeManager.Instance.DialBoxController.ClearAll();
                NarrativeManager.Instance.TriggerNarrative(_dialBoxID);
                gameObject.SetActive(false);
            }
        }
    }
}
