using System.Threading.Tasks;
using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.InGameRepositories.Interface;
using Services.LevelLoader.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
    public class LoadManager : MonoBehaviour
    {
        private IGameConfigService _gameConfigService;
        private IEventService _eventService;
        private IFactoryService _factoryService;
        private ILevelLoaderService _levelLoaderService;
        private IInGameRepositoryService _inGameRepositoryService;

        [Inject]
        private void Init(IGameConfigService gameConfigService,
            IEventService eventService,
            IFactoryService factoryService,
            ILevelLoaderService levelLoaderService,
            IInGameRepositoryService inGameRepositoryService)
        {
            _gameConfigService = gameConfigService;
            _eventService = eventService;
            _factoryService = factoryService;
            _levelLoaderService = levelLoaderService;
            _inGameRepositoryService = inGameRepositoryService;
        }

        private void Awake()
        {
            LoadGame();
        }

        private async void LoadGame()
        {
            await _factoryService.LoadUiPanels();
            await _factoryService.LoadUiElements();
            await _factoryService.LoadBalls();

            _gameConfigService.LoadGameConfig();

            while (_gameConfigService.IsConfigLoaded.ActionState == ActionResultType.InProgress)
            {
                await Task.Delay(10);
            }

            if (_gameConfigService.IsConfigLoaded.ActionState == ActionResultType.Fail)
            {
                //todo reload game
                SceneManager.LoadScene(0);
                return;
            }

            _eventService.Fire(GameEvents.ON_GAME_INITIALIZED, new OnGameInitialized());
            _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.SplashPlace).gameObject.SetActive(false);
            _levelLoaderService.LoadCurrentLevel();
        }
    }
}