using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.EventSystem.Interface;
using Services.UISystem.Interface;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private int _ballNeededToFinishCount;
        private int _secondStarBallCount;
        private int _thirdStarBallCount;
        private int _inCupBallCount;

        private IEventService _eventService;
        private IUIService _uiService;

        [Inject]
        private void Init(IEventService eventService,
            IUIService uiService)
        {
            _eventService = eventService;
            _uiService = uiService;

            _eventService.Subscribe<OnLoadLevelStart>(GameEvents.ON_LOAD_LEVEL_START, OnLoadLevelStart);
            _eventService.Subscribe<OnLoadLevelComplete>(GameEvents.ON_LOAD_LEVEL_COMPLETE, OnLoadLevelComplete);
            _eventService.Subscribe<OnBallEnteredCup>(GameEvents.ON_BALL_ENTERED_CUP, OnBallEnteredCup);
        }

        private void OnLoadLevelStart(OnLoadLevelStart onLoadLevelStart)
        {
            _inCupBallCount = _ballNeededToFinishCount = _secondStarBallCount = _thirdStarBallCount = 0;
            _uiService.OpenPage(UiPanelNames.UIGame, null, null);
        }

        private void OnLoadLevelComplete(OnLoadLevelComplete onLoadLevelComplete)
        {
            _ballNeededToFinishCount = onLoadLevelComplete.BallNeededToFinish;
            _secondStarBallCount = onLoadLevelComplete.BallNeededForSecondStar;
            _thirdStarBallCount = onLoadLevelComplete.BallNeededForThirdStar;
        }

        private void OnBallEnteredCup(OnBallEnteredCup onBallEnteredCup)
        {
            _inCupBallCount++;

            bool canFinishLevel = _inCupBallCount >= _ballNeededToFinishCount;

            float firstStarProgress = (float)_inCupBallCount / (float)_ballNeededToFinishCount;
            float secondStarProgress = 0;
            float thirdStarProgress = 0;

            if (firstStarProgress >= 1)
            {
                firstStarProgress = 1;

                secondStarProgress = (float)_inCupBallCount / (float)(_secondStarBallCount);

                if (secondStarProgress >= 1)
                {
                    secondStarProgress = 1;
                    thirdStarProgress = (float)_inCupBallCount / (float)(_thirdStarBallCount);

                    if (thirdStarProgress >= 1)
                    {
                        thirdStarProgress = 1;
                    }
                }
            }

            _eventService.Fire(GameEvents.ON_BALL_COUNT_CHANGED,
                new OnBallCountChanged(_inCupBallCount, canFinishLevel,
                    firstStarProgress, secondStarProgress, thirdStarProgress));
        }
    }
}