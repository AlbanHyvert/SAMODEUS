using UnityEngine;

public class RunesDoors : MonoBehaviour
{
    [SerializeField ,Header("Movable Runes Array")] private GameObject[] _runesGO = null;
    [SerializeField ,Header("Runes References")] private GameObject[] _runesOrder = null;

    private void Update()
    {

        if (Mathf.Approximately(_runesGO[0].transform.eulerAngles.z, _runesOrder[0].transform.eulerAngles.z))
        {
            Debug.Log("Check1");
            _runesGO[0].GetComponent<IAction>().Exit();
        }

        if (Mathf.Approximately(_runesGO[1].transform.eulerAngles.z, _runesOrder[1].transform.eulerAngles.z))
        {
            Debug.Log("Check2");
            _runesGO[1].GetComponent<IAction>().Exit();
        }

        if (Mathf.Approximately(_runesGO[2].transform.eulerAngles.z, _runesOrder[2].transform.eulerAngles.z))
        {
            Debug.Log("Check3");
            _runesGO[2].GetComponent<IAction>().Exit();
        }
    }
}
