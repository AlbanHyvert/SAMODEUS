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
        GAME
    }

    private GameState _currentState = GameState.PRELOAD;
    private Dictionary<GameState, IGameState> _states = null;
    private Rigidbody _physics = null;
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
        _states.Add(GameState.GAME, new global::GameState());
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
