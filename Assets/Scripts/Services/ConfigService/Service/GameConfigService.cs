using System.Threading.Tasks;
using Domain.Constants;
using Domain.Data;
using Domain.Enum;
using Services.ConfigService.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.ConfigService.Service
{
    public class GameConfigService : IGameConfigService
    {
        private readonly ActionResult _isConfigLoaded;
        private GameConfig _gameConfig;
        private float _loadProgress;

        public ActionResult IsConfigLoaded => _isConfigLoaded;
        public float LoadProgress => _loadProgress;
        public int MaxLevelCount => _gameConfig.LevelSequence.AllLevelsData.Length;

        public GameConfigService()
        {
            _isConfigLoaded = new ActionResult(ActionResultType.InProgress, string.Empty, -1);
        }

        public async void LoadGameConfig()
        {
            var asyncLoadGameConfig = Addressables.LoadAssetAsync<GameConfig>(AddressableKeys.GAME_CONFIG);

            while (!asyncLoadGameConfig.IsDone)
            {
                _loadProgress = asyncLoadGameConfig.PercentComplete;
                await Task.Delay(10);
            }

            _loadProgress = 1;
            if (asyncLoadGameConfig.Status == AsyncOperationStatus.Failed)
            {
                //todo Reload Game Again
                return;
            }

            _gameConfig = asyncLoadGameConfig.Result;

            if (_gameConfig == null)
            {
                //todo Reload Game Again
                _isConfigLoaded.ChangeResult(ActionResultType.Fail, string.Empty, 400);
                return;
            }

            _isConfigLoaded.ChangeResult(ActionResultType.Success, string.Empty, 100);
        }

        public LevelData GetLevelData(int levelIndex)
        {
            return _gameConfig.LevelSequence.AllLevelsData[levelIndex];
        }
    }
}