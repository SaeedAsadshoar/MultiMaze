using System.Threading.Tasks;
using Domain.Constants;
using Domain.Enum;
using Domain.Events;
using Presentation.UI;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.LevelLoader.Interface;
using Services.UISystem.Interface;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class LoadManager : MonoBehaviour
    {
        private IUIService _uiService;
        private IGameConfigService _gameConfigService;
        private IEventService _eventService;
        private IFactoryService _factoryService;
        private ILevelLoaderService _levelLoaderService;

        [Inject]
        private void Init(IUIService uiService,
            IGameConfigService gameConfigService,
            IEventService eventService,
            IFactoryService factoryService,
            ILevelLoaderService levelLoaderService)
        {
            _uiService = uiService;
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

            var uiLoading = _uiService.OpenPage(UiPanelNames.UILoading, null, null) as UILoading;
            uiLoading.SetLoading("Load Config");
            _gameConfigService.LoadGameConfig();

            while (_gameConfigService.IsConfigLoaded.ActionState == ActionResultType.InProgress)
            {
                uiLoading.SetProgress(_gameConfigService.LoadProgress);
                await Task.Delay(10);
            }

            uiLoading.SetProgress(1);
            await Task.Delay(1000);

            if (_gameConfigService.IsConfigLoaded.ActionState == ActionResultType.Fail)
            {
                //todo reload game
                return;
            }

            if (_gameConfigService.IsConfigLoaded.ActionState == ActionResultType.Success)
            {
                //todo load current level
            }

            uiLoading.SetLoading("Opening Level");
            _levelLoaderService.LoadCurrentLevel();
            await Task.Delay(1000);
            _eventService.Fire(GameEvents.ON_GAME_INITIALIZED, new OnGameInitialized());
            await Task.Delay(1000);
            _uiService.ClosePage(UiPanelNames.UILoading, null);
        }
    }
}