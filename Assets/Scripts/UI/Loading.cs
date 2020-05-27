using Engine.Loading;
using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        Invoke("LoadScene", LoadingManager.Instance.DefaultLoadingTime);

    }

    private void LoadScene()
    {
        GameManager.Instance.ChangeState(GameManager.Instance.ChoosenScene);
    }
}
