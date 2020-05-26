using Engine.Singleton;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PRELOAD,
        MAINMENU,
        LOADING,
        GAME,
        GCF,
        VERTUMNE2,
        NE,
        PREFABS,
        GCF_HUGO,
        DEV
    }

    public enum Language
    {
        ENGLISH,
        FRENCH
    }

    [SerializeField] private GameState _gameState = GameState.GAME;

    private bool _isChangingState = false;
    private GameState _currentState = GameState.PRELOAD;
    private Dictionary<GameState, IGameStates> _states = null;

    public IGameStates CurrentStateType { get { return _states[_currentState]; } }
    public GameState ChoosenScene { get { return _gameState; } set { _gameState = value; } }

    private void Start()
    {
        _states = new Dictionary<GameState, IGameStates>();
        _states.Add(GameState.PRELOAD, new PreloadState());
        _states.Add(GameState.MAINMENU, new MainMenuState());
        _states.Add(GameState.LOADING, new LoadingState());
        _states.Add(GameState.GAME, new GameSceneState());
        _states.Add(GameState.VERTUMNE2, new VertumneState());
        _states.Add(GameState.PREFABS, new PrefabsSceneState());
        _states.Add(GameState.GCF, new GCFState());
        _states.Add(GameState.NE, new NE1State());
        _states.Add(GameState.DEV, new DevState());
        _states.Add(GameState.GCF_HUGO, new GCFHugoState());
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
