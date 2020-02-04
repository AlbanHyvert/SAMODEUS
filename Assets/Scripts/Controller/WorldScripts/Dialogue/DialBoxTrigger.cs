using UnityEngine;
using TMPro;

public class DialBoxTrigger : MonoBehaviour
{
    [SerializeField ,Header("Dials Boxs ID")] private string[] _dialBoxID = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            NarrativeManager.Instance.TriggerNarrative(_dialBoxID);
            gameObject.SetActive(false);
        }
    }
}
