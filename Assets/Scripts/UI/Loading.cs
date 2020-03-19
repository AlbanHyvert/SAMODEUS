using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        Invoke("LoadScene", GameManager.Instance.DefaultLoadingTime);

    }

    private void LoadScene()
    {
        GameManager.Instance.ChangeState(GameManager.Instance.ChoosenScene);
    }
}
