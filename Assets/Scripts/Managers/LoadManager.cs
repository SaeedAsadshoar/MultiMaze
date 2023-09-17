using System.Threading.Tasks;
using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.LevelLoader.Interface;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LoadManager : MonoBehaviour
    {
        private IGameConfigService _gameConfigService;
        private IEventService _eventService;
        private IFactoryService _factoryService;
        private ILevelLoaderService _levelLoaderService;

        [Inject]
        private void Init(IGameConfigService gameConfigService,
            IEventService eventService,
            IFactoryService factoryService,
            ILevelLoaderService levelLoaderService)
        {
            _gameConfigService = gameConfigService;
            _eventService = eventService;
            _factoryService = factoryService;
            _levelLoaderService = levelLoaderService;
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
                return;
            }

            _eventService.Fire(GameEvents.ON_GAME_INITIALIZED, new OnGameInitialized());
            _levelLoaderService.LoadCurrentLevel();
        }
    }
}