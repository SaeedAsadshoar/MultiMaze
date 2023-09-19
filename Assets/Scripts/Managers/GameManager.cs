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
        private const int DELAY_TIME_TO_SHOW_FINISH_UI = 1;
        private int _ballNeededToFinishCount;
        private int _secondStarBallCount;
        private int _thirdStarBallCount;
        private int _inCupBallCount;
        private int _inPuzzleBalls;

        private IEventService _eventService;
        private IUIService _uiService;
        private bool _isGameStarted;

        [Inject]
        private void Init(IEventService eventService,
            IUIService uiService)
        {
            _eventService = eventService;
            _uiService = uiService;

            _eventService.Subscribe<OnLoadLevelStart>(GameEvents.ON_LOAD_LEVEL_START, OnLoadLevelStart);
            _eventService.Subscribe<OnLoadLevelComplete>(GameEvents.ON_LOAD_LEVEL_COMPLETE, OnLoadLevelComplete);
            _eventService.Subscribe<OnBallEnteredCup>(GameEvents.ON_BALL_ENTERED_CUP, OnBallEnteredCup);
            _eventService.Subscribe<OnBallEnteredPuzzle>(GameEvents.ON_BALL_ENTERED_PUZZLE, OnBallEnteredPuzzle);
            _eventService.Subscribe<OnBallExitPuzzle>(GameEvents.ON_BALL_EXIT_PUZZLE, OnBallExitPuzzle);
            _eventService.Subscribe<OnGameFinished>(GameEvents.ON_GAME_FINISHED, OnGameFinished);
        }

        private void OnLoadLevelStart(OnLoadLevelStart onLoadLevelStart)
        {
            CancelInvoke(nameof(FinishGame));
            _isGameStarted = false;
            _inPuzzleBalls = _inCupBallCount = _ballNeededToFinishCount = _secondStarBallCount = _thirdStarBallCount = 0;
            _uiService.OpenPage(UiPanelNames.UIGame, null, null);
        }

        private void OnLoadLevelComplete(OnLoadLevelComplete onLoadLevelComplete)
        {
            _inPuzzleBalls = onLoadLevelComplete.MaxBalls;
            _ballNeededToFinishCount = onLoadLevelComplete.BallNeededToFinish;
            _secondStarBallCount = onLoadLevelComplete.BallNeededForSecondStar;
            _thirdStarBallCount = onLoadLevelComplete.BallNeededForThirdStar;
            _isGameStarted = true;
        }

        private void OnBallEnteredCup(OnBallEnteredCup onBallEnteredCup)
        {
            if (!_isGameStarted) return;
            _inCupBallCount++;

            bool canFinishLevel = _inCupBallCount >= _ballNeededToFinishCount;

            float firstStarProgress = (float)_inCupBallCount / (float)_ballNeededToFinishCount;
            float secondStarProgress = 0;
            float thirdStarProgress = 0;

            if (firstStarProgress >= 1)
            {
                firstStarProgress = 1;

                secondStarProgress = (float)(_inCupBallCount - _ballNeededToFinishCount) / (float)(_secondStarBallCount);

                if (secondStarProgress >= 1)
                {
                    secondStarProgress = 1;
                    thirdStarProgress = (float)(_inCupBallCount - _ballNeededToFinishCount - _secondStarBallCount) / (float)(_thirdStarBallCount);

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

        private void OnBallEnteredPuzzle(OnBallEnteredPuzzle onBallEntered)
        {
            if (!_isGameStarted) return;
            _inPuzzleBalls++;
        }

        private void OnBallExitPuzzle(OnBallExitPuzzle onBallExitPuzzle)
        {
            if (!_isGameStarted) return;
            _inPuzzleBalls--;
            if (_inPuzzleBalls <= 0)
            {
                _uiService.ClosePage(UiPanelNames.UIGame, null);
                Invoke(nameof(FinishGame), DELAY_TIME_TO_SHOW_FINISH_UI);
                return;
            }

            if (CanFinishGame()) return;

            int minNeededBalls = (_inCupBallCount - _ballNeededToFinishCount);
            if (minNeededBalls > _inPuzzleBalls)
            {
                _isGameStarted = false;
                _eventService.Fire(GameEvents.ON_GAME_FINISHED, new OnGameFinished());
                _uiService.ClosePage(UiPanelNames.UIGame, null);
                _uiService.OpenPage(UiPanelNames.UILevelFailed, null, null);
            }
        }

        private void OnGameFinished(OnGameFinished onGameFinished)
        {
            _isGameStarted = false;
        }

        private void FinishGame()
        {
            _eventService.Fire(GameEvents.ON_GAME_FINISHED, new OnGameFinished());
            _uiService.OpenPage(CanFinishGame() ? UiPanelNames.UILevelPassed : UiPanelNames.UILevelFailed, null, null);
        }

        private bool CanFinishGame()
        {
            return _inCupBallCount >= _ballNeededToFinishCount;
        }
    }
}