using Unity.VisualScripting;

namespace CodeBase.GameStateMachine
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly LoadScreen _loadScreen;

        public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadScreen loadScreen)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadScreen = loadScreen;
        }

        public void Enter(string sceneName)
        {
            _loadScreen.Show();
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            _loadScreen.Hide();
        }

        private void OnLoaded()
        {
            _gameStateMachine.Enter<GameLoopState>();
            // Можно что-то сделать при загрузке уровня (заспавнить игрока, худ)
        }
    }
}