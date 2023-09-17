using Domain.Enum;
using Services.LevelLoader.Interface;
using Services.UISystem.Abstract;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Presentation.UI
{
    public class UILevelPassed : EachUIPanel<UILevelPassed>
    {
        [SerializeField] private Button _nextLevelButton;

        private ILevelLoaderService _levelLoaderService;
        public override UiPanelNames PanelName => UiPanelNames.UILevelFailed;

        [Inject]
        private void Init(ILevelLoaderService levelLoaderService)
        {
            _levelLoaderService = levelLoaderService;
        }

        private void Awake()
        {
            _nextLevelButton.onClick.AddListener(NextLevelButtonClicked);
        }

        private void NextLevelButtonClicked()
        {
            _levelLoaderService.LoadNextLevel();
        }
    }
}