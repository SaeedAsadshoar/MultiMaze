using Domain.Constants;
using Domain.Data;
using Domain.Enum;
using Services.Config.Interface;
using UnityEngine;

namespace Services.Config.Service
{
    public class GameConfigService : IGameConfigService
    {
        private readonly ActionResult _isConfigLoaded;
        private readonly GameConfig _gameConfig;

        public ActionResult IsConfigLoaded => _isConfigLoaded;

        public GameConfigService()
        {
            _isConfigLoaded = new ActionResult(ActionResultType.InProgress, string.Empty, -1);
            _gameConfig = Resources.Load<GameConfig>(ResourcesPath.GAME_CONFIG);
            if (_gameConfig != null)
            {
                _isConfigLoaded.ChangeResult(ActionResultType.Success, string.Empty, 100);
            }
            else
            {
                _isConfigLoaded.ChangeResult(ActionResultType.Fail, string.Empty, 400);
            }
        }
    }
}