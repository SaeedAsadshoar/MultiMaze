using System;
using Domain.Enum;
using Services.LevelLoader.Interface;
using Services.UISystem.Abstract;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Presentation.UI
{
    public class UILevelFailed : EachUIPanel<UILevelFailed>
    {
        [SerializeField] private Button _restartLevelButton;

        private ILevelLoaderService _levelLoaderService;
        public override UiPanelNames PanelName => UiPanelNames.UILevelFailed;

        [Inject]
        private void Init(ILevelLoaderService levelLoaderService)
        {
            _levelLoaderService = levelLoaderService;
        }

        private void Awake()
        {
            _restartLevelButton.onClick.AddListener(RestartLevelButtonClicked);
        }

        private void RestartLevelButtonClicked()
        {
            _levelLoaderService.RestartLevel();
        }
    }
}