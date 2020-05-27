using UnityEngine;
using Engine.Singleton;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Engine.Loading
{
    public class LoadingManager : Singleton<LoadingManager>
    {
        [SerializeField] private int _defaultLoadingTime = 1;

        private AsyncOperation _asyncOperationLoad = null;
        private AsyncOperation _asyncOperationUnLoad = null;
        private Scene _scene;
        private string _sceneToUnload = string.Empty;
        private string _sceneNameLoad = string.Empty;
        private string _sceneNameUnLoad = string.Empty;
        private string[] _sceneNames = null;
        private bool _alreadyUsed = false;
        private int _sceneFinishedToLoad = 0;
        private int _i = 0;

        public string SceneToUnload { get { return _sceneToUnload; } set { _sceneToUnload = value; } }
        public int DefaultLoadingTime { get { return _defaultLoadingTime; } }

        private void Start()
        {
            _asyncOperationLoad = null;
            _sceneFinishedToLoad = 0;
            _i = 0;
            _alreadyUsed = false;
        }

        public void LoadScene(string sceneName)
        {
            _asyncOperationLoad = SceneManager.LoadSceneAsync(sceneName);
        }

        public void LoadScene(string[] sceneNames)
        {
            _sceneNames = sceneNames;
            GameLoopManager.Instance.Managers += OnUpdate;
        }

        public void UnloadScene(string[] sceneArray)
        {
            _sceneNames = sceneArray;

            foreach (string sceneName in _sceneNames)
            {
                if (sceneName != string.Empty && SceneManager.GetSceneByName(sceneName).isLoaded)
                {
                    _sceneNameUnLoad = sceneName;
                    StartCoroutine(UnLoadCoroutine(sceneName));
                }
            }
        }

        public void UnloadScene(string sceneName)
        {
            _asyncOperationUnLoad = SceneManager.UnloadSceneAsync(sceneName);
        }

        private void OnUpdate()
        {
            LoadingScene();
            GameLoopManager.Instance.Managers -= OnUpdate;
        }

        private void LoadingScene()
        {
            if(_sceneNames != null)
            {
                if( _i < _sceneNames.Length && _sceneNames[_i] != null)
                {
                    _scene = SceneManager.GetSceneByName(_sceneNames[_i]);

                    if(_scene.isLoaded == false)
                    {
                        StartCoroutine(OnLoadCoroutine(_i));
                    }
                }
                else if( _sceneFinishedToLoad == _sceneNames.Length && _sceneToUnload != string.Empty)
                {
                    UnloadScene(_sceneToUnload);
                }
            }
        }

        IEnumerator OnLoadCoroutine(int i)
        {
            yield return new WaitForSeconds(0.3f);
            if(_sceneNames[i] != string.Empty)
            {
                _asyncOperationLoad = SceneManager.LoadSceneAsync(_sceneNames[i], LoadSceneMode.Additive);
                _asyncOperationLoad.completed += OnCompleted;
            }
            yield return null;
        }

        IEnumerator UnLoadCoroutine(string sceneName)
        {
            _asyncOperationUnLoad = SceneManager.UnloadSceneAsync(sceneName);
            //_asyncOperationUnLoad.completed += OnUnLoadCompleted;
            yield return null;
        }

        private void OnCompleted(AsyncOperation async)
        {
            _sceneFinishedToLoad += 1;
            StopCoroutine(OnLoadCoroutine(_i));

            if(_sceneNames.Length == _sceneFinishedToLoad)
            {
                // stop pause
                UnloadScene(_sceneToUnload);
            }
            else
            {
                _i += 1;
                LoadingScene();
                _asyncOperationLoad.completed -= OnCompleted;
            }

            _asyncOperationLoad = null;
        }

        private void OnUnLoadCompleted(AsyncOperation async)
        {
            StopCoroutine(UnLoadCoroutine(_sceneNameUnLoad));
            _asyncOperationUnLoad.completed -= OnUnLoadCompleted;
            _asyncOperationUnLoad = null;
        }
    }
}