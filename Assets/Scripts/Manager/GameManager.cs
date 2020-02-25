using System.Collections.Generic;
using Engine.Singleton;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    #region Fields
    public enum GameState
    {
        PRELOAD,
        MAINMENU,
        GAME,
        GCF,
        VERTUMNE,
        VERTUMNE_PREFABS,
        LOADING
    }
    private GameState _currentState = GameState.PRELOAD;
    private Dictionary<GameState, IGameState> _states = null;
    #endregion Fields

    #region Properties
    public GameState CurrentState { get { return _currentState; } }
    public IGameState CurrentStateType { get { return _states[_currentState]; } }
    #endregion Properties

    #region Methods

    private void Start()
    {
        _states = new Dictionary<GameState, IGameState>();
        _states.Add(GameState.PRELOAD, new PreloadState());
        _states.Add(GameState.MAINMENU, new MainMenuState());
        _states.Add(GameState.LOADING, new LoadingState());
        _states.Add(GameState.GAME, new InGameState());
        _states.Add(GameState.GCF, new GCF_1State());
        _states.Add(GameState.VERTUMNE, new VertumneState());
        _states.Add(GameState.VERTUMNE_PREFABS, new VertumnePrefabsState());
        _currentState = GameState.PRELOAD;
        ChangeState(GameState.MAINMENU);
    }

    public void ChangeState(GameState nextState)
    {
        _states[_currentState].Exit();
        _states[nextState].Enter();
        _currentState = nextState;
    }
    #endregion Methods
}
