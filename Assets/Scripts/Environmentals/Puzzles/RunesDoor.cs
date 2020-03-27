using UnityEngine;

public class RunesDoor : MonoBehaviour
{
    [SerializeField] Runes[] _runesList = null;
    [SerializeField] private Transform[] _matchingRunes = null;
    [SerializeField] private GameObject[] _desactivedObject = null;

    private void Start()
    {
        /*for (int i = 0; i < _matchingRunes.Length; i++)
          {
              _runesList[i].MatchingValueX = (int)_matchingRunes[i].eulerAngles.z;
          }
          GameLoopManager.Instance.Puzzles += OnUpdate;*/
    }

    private void OnUpdate()
    {
      /*for (int i = 0; i < _runesList.Length; i++)
        {
            if(_runesList[0].IsMatching == true && _runesList[1].IsMatching == true && _runesList[2].IsMatching == true)
            {
                Debug.Log("Matching");
                GameLoopManager.Instance.Puzzles -= OnUpdate;

                if(_desactivedObject != null)
                {
                    for (int j = 0; j < _desactivedObject.Length; j++)
                    {
                        _desactivedObject[j].SetActive(false);
                    }
                }
            }
        }*/
    }
}
