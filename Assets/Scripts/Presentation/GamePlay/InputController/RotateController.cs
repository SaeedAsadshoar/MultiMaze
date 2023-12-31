using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.InGameRepositories.Interface;
using Services.UpdateSystem.Interface;
using UnityEngine;
using Zenject;

namespace Presentation.GamePlay.InputController
{
    public class RotateController : MonoBehaviour
    {
        private Camera _mainCamera;

        private IInGameRepositoryService _inGameRepositoryService;
        private IGameConfigService _gameConfigService;

        private Transform _levelPlaceTransform;

        private float _startValue;
        private bool _canRotate;
        private Vector3 _rotationCenterPos;
        private Vector3 _preTouchPos;
        private float _rotationSpeed;
        private float _deltaDegree;

        [Inject]
        private void Init(IInGameRepositoryService inGameRepositoryService,
            IEventService eventService,
            IUpdateService updateService,
            IGameConfigService gameConfigService)
        {
            _inGameRepositoryService = inGameRepositoryService;
            _gameConfigService = gameConfigService;

            eventService.Subscribe<OnLoadLevelStart>(GameEvents.ON_LOAD_LEVEL_START, OnStartLoadLevel);
            eventService.Subscribe<OnLoadLevelComplete>(GameEvents.ON_LOAD_LEVEL_COMPLETE, OnLoadLevelComplete);
            eventService.Subscribe<OnGameFinished>(GameEvents.ON_GAME_FINISHED, OnGameFinished);

            updateService.AddToUpdate(UpdateFunc, UnityUpdateType.Update);
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
            _levelPlaceTransform ??= _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlace);
        }

        void UpdateFunc()
        {
            if (!_canRotate) return;
            if (Input.GetMouseButtonDown(0))
            {
                _startValue = CalculateRotationDegree(Input.mousePosition);
                _preTouchPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                var currentDegree = CalculateRotationDegree(Input.mousePosition);
                var degree = currentDegree - _startValue;

                //bug in left side - unity suddenly return 359 or -359 we check it here for prevent bug
                if (degree is < 10 and > -10)
                {
                    _deltaDegree = degree;
                }

                var distance = Vector3.Distance(Input.mousePosition, _preTouchPos);
                _rotationSpeed = distance * _gameConfigService.TouchSetting.DeltaDistanceRatio;

                distance = Vector3.Distance(Input.mousePosition, _rotationCenterPos);
                _deltaDegree *= distance * _gameConfigService.TouchSetting.CenterDistanceRatio;
                _startValue = currentDegree;
            }
            else
            {
                _deltaDegree = Mathf.Lerp(_deltaDegree, 0, _gameConfigService.TouchSetting.StopRotateSpeed * Time.deltaTime);
                _rotationSpeed = Mathf.Lerp(_rotationSpeed, 0, _gameConfigService.TouchSetting.StopRotateSpeed * Time.deltaTime);
            }

            _levelPlaceTransform.Rotate(Vector3.forward, _deltaDegree * _rotationSpeed);
        }

        private void OnStartLoadLevel(OnLoadLevelStart onLoadLevelStart)
        {
            Reset();
            _canRotate = false;
        }

        private void OnLoadLevelComplete(OnLoadLevelComplete onLoadLevelComplete)
        {
            Reset();
            _rotationCenterPos = _mainCamera.WorldToScreenPoint(_levelPlaceTransform.position);
            _canRotate = true;
        }

        private void OnGameFinished(OnGameFinished onGameFinished)
        {
            _canRotate = false;
        }

        private void Reset()
        {
            _rotationSpeed = _deltaDegree = _rotationCenterPos.z = 0;
            _levelPlaceTransform.rotation = Quaternion.identity;
        }

        private float CalculateRotationDegree(Vector3 touchPosition)
        {
            var movement = touchPosition - _rotationCenterPos;
            var target = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            return target;
        }
    }
}