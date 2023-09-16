using System;
using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Services.EventSystem.Interface;
using Services.InGameRepositories.Interface;
using Services.UpdateSystem.Interface;
using UnityEngine;
using Zenject;

namespace Presentation.GamePlay.InputController
{
    public class RotateController : MonoBehaviour
    {
        [SerializeField] private float _rotateSpeed = 5;

        private Camera _mainCamera;

        private IInGameRepositoryService _inGameRepositoryService;
        private Transform _levelPlaceTransform;
        private Rigidbody _levelPlaceRigidbody;

        private float _startValue;

        private Quaternion _destRotation;
        private bool _canRotate;

        [Inject]
        private void Init(IInGameRepositoryService inGameRepositoryService,
            IEventService eventService,
            IUpdateService updateService)
        {
            _inGameRepositoryService = inGameRepositoryService;
            eventService.Subscribe<OnLoadLevelStart>(GameEvents.ON_LOAD_LEVEL_START, OnStartLoadLevel);
            eventService.Subscribe<OnLoadLevelComplete>(GameEvents.ON_LOAD_LEVEL_COMPLETE, OnLoadLevelComplete);

            updateService.AddToUpdate(UpdateFunc, UnityUpdateType.Update);
            updateService.AddToUpdate(FixedUpdateFunc, UnityUpdateType.FixedUpdate);
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
            }

            if (Input.GetMouseButton(0))
            {
                var currentDegree = CalculateRotationDegree(Input.mousePosition);
                var delta = currentDegree - _startValue;
                _destRotation = Quaternion.Euler(0, 0, _destRotation.eulerAngles.z + delta);
                _startValue = currentDegree;
            }
        }

        private void FixedUpdateFunc()
        {
            if (!_canRotate) return;
            _levelPlaceTransform.rotation = Quaternion.Slerp(_levelPlaceTransform.rotation, _destRotation, _rotateSpeed * Time.deltaTime);
            //_levelPlaceRigidbody.MoveRotation(_destRotation);
        }

        private void OnStartLoadLevel(OnLoadLevelStart onLoadLevelStart)
        {
            _canRotate = false;
            _destRotation = Quaternion.identity;
        }

        private void OnLoadLevelComplete(OnLoadLevelComplete onLoadLevelComplete)
        {
            _levelPlaceRigidbody = _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlaceRigidBody).GetComponent<Rigidbody>();
            _canRotate = true;
            _destRotation = Quaternion.identity;
        }

        private float CalculateRotationDegree(Vector3 touchPosition)
        {
            Vector3 movePos = new Vector3(touchPosition.x, touchPosition.y, 0);
            var objectPos = _mainCamera.WorldToScreenPoint(_levelPlaceTransform.position);
            objectPos.z = 0;
            var movement = movePos - objectPos;
            var target = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            return target;
        }
    }
}