using Engine.Singleton;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneAsyncManager : Singleton<SceneAsyncManager>
{
    [SerializeField] private int _defaultLoadingTime = 1;

    private AsyncOperation _asyncStatus = null;
    private Scene _scene;
    private bool _asEndedLoading = false;
    private bool _isLoadingAScene = false;
    private bool _isUnloadingAScene = false;
    private string[] _scenesNames = null;
    private string[] _oldSceneNames = null;
    private string _oldSceneName = string.Empty;
    private int _i = 0;
    private int _j = 0;

    public int DefaultLoadingTime { get { return _defaultLoadingTime; } }
    public AsyncOperation AsyncStatus { get { return _asyncStatus; } }
    public bool AsEndedLoading { get { return _asEndedLoading; } }

    private void Start()
    {
        GameLoopManager.Instance.Managers += OnUpdate;
    }

    public void LoadScene(string sceneName)
    {
        _asEndedLoading = false;
        _asyncStatus = SceneManager.LoadSceneAsync(sceneName);
        _oldSceneName = sceneName;
        _isLoadingAScene = true;
    }

    public void LoadScenes(string[] sceneNames)
    {
        _asEndedLoading = false;
        _scenesNames = sceneNames;
        _i = 0;
        _isLoadingAScene = true;
    }

    public void UnloadScene(string sceneName)
    {
        _asEndedLoading = false;
        _asyncStatus = SceneManager.UnloadSceneAsync(sceneName);
        _oldSceneName = sceneName;
        _isUnloadingAScene = true;
    }

    public void UnloadScenes(string[] sceneNames)
    {
        _asEndedLoading = false;
        _scenesNames = sceneNames;
        _j = 0;
        _isUnloadingAScene = true;
    }

    private void OnUpdate()
    {
        if (_isLoadingAScene == true && _scenesNames != null)
        {
            LoadingScenes();
        }
        else if (_isLoadingAScene == true && _scenesNames == null)
        {
            LoadingScene();
        }

        if (_isUnloadingAScene == true && _scenesNames != null)
        {
            //UnLoadingScenes();
        }
        else if (_isUnloadingAScene == true && _scenesNames == null)
        {
            UnloadingScene();
        }
    }

    private void LoadingScenes()
    {
        if (_asyncStatus == null)
        {
            _asyncStatus = SceneManager.LoadSceneAsync(_scenesNames[_i], LoadSceneMode.Additive);
            _scene = SceneManager.GetSceneByName(_scenesNames[_i]);
        }

        if (_asyncStatus.isDone == true && _i < _scenesNames.Length - 1)
        {
            if (_i == 1)
            {
                if (PlayerManager.Instance.Player != null)
                    SceneManager.MoveGameObjectToScene(PlayerManager.Instance.Player.gameObject, SceneManager.GetSceneByName(_scenesNames[0]));
                PlayerManager.Instance.Player.transform.position = PlayerManager.Instance.PlayerStartingPosition.position;
            }
            _asyncStatus = null;
            _i++;
            return;
        }
        else if (_i >= _scenesNames.Length - 1 && _asyncStatus.isDone == true)
        {
            _i = 0;
            _asEndedLoading = true;
            _scenesNames = null;
            _asyncStatus = null;
            UnloadScene(_oldSceneName);
            _isLoadingAScene = false;
        }
    }

    private void LoadingScene()
    {
        if (_asyncStatus != null)
        {
            if (_asyncStatus.isDone == true)
            {
                _asEndedLoading = true;
                _asyncStatus = null;
                _isLoadingAScene = false;
            }
        }
    }

    private void UnLoadingScenes()
    {
        if (_asyncStatus == null)
        {
            _asyncStatus = SceneManager.UnloadSceneAsync(_scenesNames[_j]);
            _scene = SceneManager.GetSceneByName(_scenesNames[_j]);
        }

        if (_asyncStatus.isDone == true && _j < _scenesNames.Length - 1)
        {
            _asyncStatus = null;
            _j++;
            return;
        }
        else if (_j >= _scenesNames.Length - 1 && _asyncStatus.isDone == true)
        {
            _j = 0;
            _asEndedLoading = true;
            _scenesNames = null;
            _asyncStatus = null;
            _isUnloadingAScene = false;
        }
    }

    private void UnloadingScene()
    {
        if (_asyncStatus != null)
        {
            if (_asyncStatus.isDone == true)
            {
                _asEndedLoading = true;
                _asyncStatus = null;
                _isUnloadingAScene = false;
            }
        }
    }
}