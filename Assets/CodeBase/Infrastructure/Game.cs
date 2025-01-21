namespace CodeBase.Infrastructure
{
    public class Game
    {
        public GameStateMachine.GameStateMachine _stateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadScreen loadScreen)
        {
            _stateMachine = new GameStateMachine.GameStateMachine(new SceneLoader(coroutineRunner), loadScreen);
        }
    }
}