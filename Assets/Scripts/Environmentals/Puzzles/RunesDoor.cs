using UnityEngine;

public class RunesDoor : MonoBehaviour
{
    [SerializeField] Runes[] _runesList = null;
    [SerializeField] private Transform[] _matchingRunes = null;
    [SerializeField] private GameObject[] _desactivedObject = null;
    [SerializeField, Range(1, 4)] private int[] _matchingValue = null;

    private void Start()
    {
        for (int i = 0; i < _matchingRunes.Length; i++)
        {
            if(_matchingValue[i] == 1)
            {
                _matchingRunes[i].rotation = Quaternion.Euler(0, 0, 90);
            }
            else if(_matchingValue[i] == 2)
            {
                _matchingRunes[i].rotation = Quaternion.Euler(0, 0, 30);
            }
            else if (_matchingValue[i] == 3)
            {
                _matchingRunes[i].rotation = Quaternion.Euler(0, 0, 10);
            }
            else if(_matchingValue[i] == 4)
            {
                _matchingRunes[i].rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        GameLoopManager.Instance.Puzzles += OnUpdate;

    }

    private void OnUpdate()
    {
      for (int i = 0; i < _runesList.Length; i++)
        {
            if(_runesList[0].PositionValue == _matchingValue[0] && _runesList[1].PositionValue == _matchingValue[1] && _runesList[2].PositionValue == _matchingValue[2])
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
        }
    }
}
