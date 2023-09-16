using Domain.Enum;

namespace Domain.GameEvents
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