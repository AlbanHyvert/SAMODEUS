using UnityEngine;

public class Loading : MonoBehaviour
{
    private void Start()
    {
        Invoke("LoadScene", SceneAsyncManager.Instance.DefaultLoadingTime);

    }

    private void LoadScene()
    {
        GameManager.Instance.ChangeState(GameManager.Instance.ChoosenScene);
    }
}
