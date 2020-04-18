using Engine.Singleton;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneAsyncManager : Singleton<SceneAsyncManager>
{
    private AsyncOperation _asyncOperation = null;
    private int _i = 0;
    private bool _asEndedLoading = false;
    private string[] _scenesName = null;
    private string _oldSceneName = string.Empty;
    private Scene _scene;

    public AsyncOperation AsyncOperation { get { return _asyncOperation; } }
    public bool AsEndedLoading { get { return _asEndedLoading; } }

    public void LoadScene(string sceneName)
    {
        _asEndedLoading = false;
        _asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        _oldSceneName = sceneName;
    }

    public void LoadScenes(string[] scenesName)
    {
        _asEndedLoading = false;
        _scenesName = scenesName;
        _i = 0;
    }

    public void UnloadScene(string sceneName)
    {
        _asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
    }

    public void UnloadScenes(string[] scenesName)
    {
        /*_asEndedLoading = false;

        for (_i = 0; _i < scenesName.Length;)
        {
            if (_asyncOperation == null)
            {
                _asyncOperation = SceneManager.UnloadSceneAsync(scenesName[_i]);
            }

            if (_asyncOperation.isDone == true)
            {
                _i++;
            }
        }

        _asEndedLoading = true;
        _asyncOperation = null;*/
    }

    private void Update()
    {
        

        if (_scenesName != null)
        {
            if (_asyncOperation == null)
            {
                _asyncOperation = SceneManager.LoadSceneAsync(_scenesName[_i], LoadSceneMode.Additive);
                _scene = SceneManager.GetSceneByName(_scenesName[_i]);
            }
            if (_asyncOperation.isDone == true && _i < _scenesName.Length - 1)
            {
                if (_i == 0)
                    SceneManager.MoveGameObjectToScene(PlayerManager.Instance.Player.gameObject, _scene);

                _asyncOperation = null;
                _i++;
                return;
            }

            if (_i >= _scenesName.Length - 1 && _asyncOperation.isDone == true)
            {
                _i = 0;
                _asEndedLoading = true;
                _scenesName = null;
                _asyncOperation = null;
                _asyncOperation = SceneManager.UnloadSceneAsync(_oldSceneName);
            }
        }
        else if (_scenesName == null && _asyncOperation != null)
        {
            if (_asyncOperation.isDone == true)
            {
                _asEndedLoading = true;
                _asyncOperation = null;
            }
        }
    }
}
