using CodeBase.GameStateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        public LoadScreen _loadScreen;
        
        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _loadScreen);
            _game._stateMachine.Enter<BootstrapState>();
            
            DontDestroyOnLoad(this);
        }
    }
}