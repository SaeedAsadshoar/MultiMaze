using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Constants;
using Domain.Data;
using Domain.Enum;
using Domain.GameEvents;
using Extension;
using Presentation.GamePlay.Balls.Interface;
using Presentation.GamePlay.Levels.Interface;
using Presentation.UI;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.InGameRepositories.Interface;
using Services.LevelLoader.Interface;
using Services.StorageSystem.Interface;
using Services.UISystem.Interface;
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
        private readonly IUIService _uiService;
        private UILoading _uiLoading;

        public LevelLoaderService(IStorageService storageService,
            IGameConfigService gameConfigService,
            IInGameRepositoryService inGameRepositoryService,
            IUpdateService updateService,
            IFactoryService factoryService,
            IEventService eventService,
            IUIService uiService)
        {
            _createdBalls = new List<IBall>();
            _storageService = storageService;
            _gameConfigService = gameConfigService;
            _inGameRepositoryService = inGameRepositoryService;
            _updateService = updateService;
            _factoryService = factoryService;
            _eventService = eventService;
            _uiService = uiService;
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

        private async void LoadLevel(int index)
        {
            _uiService.ClosePage(UiPanelNames.UILevelFailed, null);
            _uiService.ClosePage(UiPanelNames.UILevelPassed, null);
            _uiLoading = _uiService.OpenPage(UiPanelNames.UILoading, null, null) as UILoading;
            _uiLoading.SetLoading($"Load Level {index + 1}");
            _uiLoading.SetProgress(0);

            await Task.Delay(1000);

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

            await Task.Delay(500);

            _inGameRepositoryService.AddRepository((int)InGameRepositoryTypes.LevelPlaceRigidBody, _currentLevelObj.GetComponent<ILevel>().LevelRigidbody.transform);
            _uiLoading.SetProgress(1);
            _uiLoading.SetLoading("Load Balls");
            CreateBalls();
        }

        private void CreateBalls()
        {
            if (_createdBalls.Count < _currentLevelData.BallCountInBase)
            {
                CreateBall();
                _updateService.DoInNextFrame(CreateBalls);
                _uiLoading.SetProgress((float)_createdBalls.Count / (float)_currentLevelData.BallCountInBase);
                foreach (var ball in _createdBalls)
                {
                    ball.ObjectRigidbody.velocity = Vector3.zero;
                }
            }
            else
            {
                OnLoadLevelFinished();
            }
        }
        
        private void CreateBall()
        {
            IBall ball = _factoryService.GetBall(BallTypes.SimpleBall) as IBall;
            _createdBalls.Add(ball);
            ball.RootTransform.SetParent(_inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallInPuzzlePlace));
            ball.RootTransform.position = _currentLevelObj.transform.position;
            ball.ObjectRigidbody.velocity = Vector3.zero;
        }

        private async void OnLoadLevelFinished()
        {
            await Task.Delay(100);
            _eventService.Fire(GameEvents.ON_LOAD_LEVEL_COMPLETE,
                new OnLoadLevelComplete(_currentLevelData.BallNeededToFinish[0],
                    _currentLevelData.BallNeededToFinish[1],
                    _currentLevelData.BallNeededToFinish[2],
                    _currentLevelData.BallCountInBase));
            _uiService.ClosePage(UiPanelNames.UILoading, null);
            _uiLoading = null;
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