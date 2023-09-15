using System.Collections.Generic;
using Domain.Constants;
using Domain.Enum;
using Extension;
using Services.ConfigService.Interface;
using Services.InGameRepositories.Interface;
using Services.LevelLoader.Interface;
using Services.StorageSystem.Interface;
using UnityEngine;

namespace Services.LevelLoader.Service
{
    public class LevelLoaderService : ILevelLoaderService
    {
        private GameObject _currentLevelObj;
        private GameObject _currentCupObj;

        private readonly IStorageService _storageService;
        private readonly IGameConfigService _gameConfigService;
        private readonly IInGameRepositoryService _inGameRepositoryService;

        public LevelLoaderService(IStorageService storageService,
            IGameConfigService gameConfigService,
            IInGameRepositoryService inGameRepositoryService)
        {
            _storageService = storageService;
            _gameConfigService = gameConfigService;
            _inGameRepositoryService = inGameRepositoryService;
        }

        public void LoadCurrentLevel()
        {
            int curlevel = int.Parse(_storageService.GetData(ConstDataNames.CURRENT_LEVEL, "0"));
            _currentLevelObj = Object.Instantiate(_gameConfigService.GetLevelObject(curlevel));
            _currentCupObj = Object.Instantiate(_gameConfigService.GetLevelCup(curlevel));

            var levelTransform = _currentLevelObj.transform;
            levelTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlace));
            levelTransform.ResetTransformation();

            var cupTransform = _currentCupObj.transform;
            cupTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.CupPlace));
            cupTransform.ResetTransformation();
        }

        public void LoadNextLevel()
        {
            throw new System.NotImplementedException();
        }

        public void RestartLevel()
        {
            throw new System.NotImplementedException();
        }
    }
}