using Engine.Singleton;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PRELOAD,
        MAINMENU,
        LOADING,
        GAME,
        GCF,
        VERTUMNE,
        NE,
        PREFABS,
        DEV
    }

    [SerializeField] private GameState _gameState = GameState.GAME;
    [SerializeField] private int _defaultLoadingTime = 2;

    private bool _isChangingState = false;
    private GameState _currentState = GameState.PRELOAD;
    private Dictionary<GameState, IGameStates> _states = null;

    public IGameStates CurrentStateType { get { return _states[_currentState]; } }
    public int DefaultLoadingTime { get { return _defaultLoadingTime; } }
    public GameState ChoosenScene { get { return _gameState; } set { _gameState = value; } }

    private void Start()
    {
        _states = new Dictionary<GameState, IGameStates>();
        _states.Add(GameState.PRELOAD, new PreloadState());
        _states.Add(GameState.MAINMENU, new MainMenuState());
        _states.Add(GameState.LOADING, new LoadingState());
        _states.Add(GameState.GAME, new GameSceneState());
        _states.Add(GameState.VERTUMNE, new VertumneState());
        _states.Add(GameState.PREFABS, new PrefabsSceneState());
        _states.Add(GameState.GCF, new GCFState());
        _states.Add(GameState.NE, new NE1State());
        _states.Add(GameState.DEV, new DevState());
        _currentState = GameState.PRELOAD;
        ChangeState(GameState.MAINMENU);
    }

    public void ChangeState(GameState nextState)
    {
        _isChangingState = true;
        _states[nextState].Enter();
        _currentState = nextState;
    }

    private void Update()
    {
        if(_isChangingState == true)
        {
            if(SceneAsyncManager.Instance.AsEndedLoading == true)
            {
                _states[_currentState].Exit();
                _isChangingState = false;
            }
        }
    }
}
