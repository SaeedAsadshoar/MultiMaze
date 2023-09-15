using Domain.Enum;

namespace Domain.Events
{
    public class OnGameStateChanged
    {
        private readonly GameStates _gameState;

        public GameStates GameState => _gameState;

        public OnGameStateChanged(GameStates gameState)
        {
            _gameState = gameState;
        }
    }
}