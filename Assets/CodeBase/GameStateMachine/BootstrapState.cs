namespace CodeBase.GameStateMachine
{
    public class BootstrapState : IState
    {
        private const string LevelScene = "Bootstrap";
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            RegisterServices();
            _sceneLoader.Load(LevelScene, EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel()
        {
            _gameStateMachine.Enter<LoadLevelState, string>("Level");
        }

        private void RegisterServices()
        {
            // Можно что-то забиндить с помощью DI
        }
    }
}