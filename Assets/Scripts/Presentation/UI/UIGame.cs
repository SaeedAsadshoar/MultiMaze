using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.EventSystem.Interface;
using Services.UISystem.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Presentation.UI
{
    public class UIGame : EachUIPanel<UIGame>
    {
        private const string COME_IN_ANIMATION = "ButtonsComeIn";
        private const string GO_OUT_ANIMATION = "ButtonsGoOut";

        [SerializeField] private Animation _endLevelButtonAnimation;
        [SerializeField] private Image _firstStarFiller;
        [SerializeField] private Image _secondStarFiller;
        [SerializeField] private Image _thirdStarFiller;

        [SerializeField] private Button _finishLevelButton;
        [SerializeField] private Button _restartLevelButton;

        [SerializeField] private TextMeshProUGUI _ballMaxCountLabel;
        [SerializeField] private TextMeshProUGUI _ballInCupCountLabel;

        private bool _isButtonOutOfScreen;
        public override UiPanelNames PanelName => UiPanelNames.UIGame;

        [Inject]
        private void Init(IEventService eventService)
        {
            eventService.Subscribe<OnLoadLevelComplete>(GameEvents.ON_LOAD_LEVEL_COMPLETE, OnLoadLevelComplete);
            eventService.Subscribe<OnBallCountChanged>(GameEvents.ON_BALL_COUNT_CHANGED, OnBallCountChanged);
        }

        private void OnEnable()
        {
            _firstStarFiller.fillAmount = 0;
            _secondStarFiller.fillAmount = 0;
            _thirdStarFiller.fillAmount = 0;

            _ballMaxCountLabel.text = string.Empty;
            _ballInCupCountLabel.text = string.Empty;

            _endLevelButtonAnimation.Play(GO_OUT_ANIMATION);
            _isButtonOutOfScreen = true;
        }

        private void OnLoadLevelComplete(OnLoadLevelComplete onLoadLevelComplete)
        {
            _ballMaxCountLabel.text = onLoadLevelComplete.BallNeededToFinish.ToString();
            _ballInCupCountLabel.text = "0";
        }

        private void OnBallCountChanged(OnBallCountChanged onBallCountChanged)
        {
            int ballCount = onBallCountChanged.InCupBallCount;
            _ballInCupCountLabel.text = ballCount.ToString();

            _firstStarFiller.fillAmount = onBallCountChanged.FirstStarProgress;
            _secondStarFiller.fillAmount = onBallCountChanged.SecondStarProgress;
            _thirdStarFiller.fillAmount = onBallCountChanged.ThirdStarProgress;

            if (onBallCountChanged.CanFinishLevel && _isButtonOutOfScreen)
            {
                _isButtonOutOfScreen = false;
                _endLevelButtonAnimation.Play(COME_IN_ANIMATION);
            }
        }
    }
}