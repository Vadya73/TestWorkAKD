using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;

namespace CodeBase.GameStateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type,IExitableState> _states;
        private IExitableState _activeState;
        private SceneLoader _sceneLoader;

        public GameStateMachine(SceneLoader sceneLoader, LoadScreen loadScreen)
        {
            _sceneLoader = sceneLoader;
            
            _states = new Dictionary<Type, IExitableState>()
            {
                [typeof(BootstrapState)] = new BootstrapState(this, _sceneLoader),
                [typeof(LoadLevelState)] = new LoadLevelState(this, _sceneLoader, loadScreen),
                [typeof(GameLoopState)] = new GameLoopState(this),
            };
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            TState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState GetState<TState>() where TState : class, IExitableState
        {
            return _states[typeof(TState)] as TState;
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
            
            return state;
        }
    }
}
