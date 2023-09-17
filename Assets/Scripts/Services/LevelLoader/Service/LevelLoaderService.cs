using System.Collections.Generic;
using Domain.Constants;
using Domain.Data;
using Domain.Enum;
using Domain.GameEvents;
using Extension;
using Presentation.GamePlay.Balls.Interface;
using Presentation.GamePlay.Levels.Interface;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.InGameRepositories.Interface;
using Services.LevelLoader.Interface;
using Services.StorageSystem.Interface;
using Services.UpdateSystem.Interface;
using UnityEngine;

namespace Services.LevelLoader.Service
{
    public class LevelLoaderService : ILevelLoaderService
    {
        private GameObject _currentLevelObj;
        private GameObject _currentCupObj;
        private readonly List<IBall> _createdBalls;
        private LevelData _currentLevelData;

        private readonly IStorageService _storageService;
        private readonly IGameConfigService _gameConfigService;
        private readonly IInGameRepositoryService _inGameRepositoryService;
        private readonly IUpdateService _updateService;
        private readonly IFactoryService _factoryService;
        private readonly IEventService _eventService;

        public LevelLoaderService(IStorageService storageService,
            IGameConfigService gameConfigService,
            IInGameRepositoryService inGameRepositoryService,
            IUpdateService updateService,
            IFactoryService factoryService,
            IEventService eventService)
        {
            _createdBalls = new List<IBall>();
            _storageService = storageService;
            _gameConfigService = gameConfigService;
            _inGameRepositoryService = inGameRepositoryService;
            _updateService = updateService;
            _factoryService = factoryService;
            _eventService = eventService;
        }


        public void LoadCurrentLevel()
        {
            int currentLevelIndex = int.Parse(_storageService.GetData(ConstDataNames.CURRENT_LEVEL, "0"));
            LoadLevel(currentLevelIndex);
        }

        public void LoadNextLevel()
        {
            int currentLevelIndex = int.Parse(_storageService.GetData(ConstDataNames.CURRENT_LEVEL, "0"));
            currentLevelIndex++;
            if (currentLevelIndex >= _gameConfigService.MaxLevelCount)
            {
                currentLevelIndex = 0;
            }

            _storageService.SetData(ConstDataNames.CURRENT_LEVEL, currentLevelIndex.ToString());
            LoadLevel(currentLevelIndex);
        }

        public void RestartLevel()
        {
            LoadCurrentLevel();
        }

        private void LoadLevel(int index)
        {
            ClearLevel();
            _eventService.Fire(GameEvents.ON_LOAD_LEVEL_START, new OnLoadLevelStart());
            _currentLevelData = _gameConfigService.GetLevelData(index);

            _currentLevelObj = Object.Instantiate(_currentLevelData.LevelPrefab);
            _currentCupObj = Object.Instantiate(_currentLevelData.Cup);

            var levelTransform = _currentLevelObj.transform;
            levelTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlace));
            levelTransform.ResetTransformation();

            var cupTransform = _currentCupObj.transform;
            cupTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.CupPlace));
            cupTransform.ResetTransformation();

            _inGameRepositoryService.AddRepository((int)InGameRepositoryTypes.LevelPlaceRigidBody, _currentLevelObj.GetComponent<ILevel>().LevelRigidbody.transform);
            CreateBalls();
        }

        private void CreateBalls()
        {
            if (_createdBalls.Count < _currentLevelData.BallCountInBase)
            {
                CreateBall();
                _updateService.DoInNextFrame(CreateBalls);
            }
            else
            {
                _eventService.Fire(GameEvents.ON_LOAD_LEVEL_COMPLETE,
                    new OnLoadLevelComplete(_currentLevelData.BallNeededToFinish[0],
                        _currentLevelData.BallNeededToFinish[1],
                        _currentLevelData.BallNeededToFinish[2]));
            }
        }

        private void CreateBall()
        {
            IBall ball = _factoryService.GetBall(BallTypes.SimpleBall) as IBall;
            _createdBalls.Add(ball);
            ball.RootTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallInPuzzlePlace));
            ball.RootTransform.position = _currentLevelObj.transform.position;

            int count = _createdBalls.Count;
            for (int i = 0; i < count; i++)
            {
                _createdBalls[i].ObjectRigidbody.velocity = Vector3.zero;
            }
        }

        private void ClearLevel()
        {
            ClearBalls();

            if (_currentLevelObj != null) Object.Destroy(_currentLevelObj);
            if (_currentCupObj != null) Object.Destroy(_currentCupObj);
        }

        private void ClearBalls()
        {
            int count = _createdBalls.Count;
            for (int i = 0; i < count; i++)
            {
                _createdBalls[i].Kill();
            }

            _createdBalls.Clear();
        }
    }
}