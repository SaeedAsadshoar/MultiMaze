using Domain.Enum;
using Services.InGameRepositories.Interface;
using UnityEngine;
using Zenject;

namespace Presentation.GamePlay
{
    public class RotateController : MonoBehaviour
    {
        private Camera _mainCamera;

        private IInGameRepositoryService _inGameRepositoryService;
        private Transform _levelPlaceTransform;
        private float _startValue;

        [Inject]
        private void Init(IInGameRepositoryService inGameRepositoryService)
        {
            _inGameRepositoryService = inGameRepositoryService;
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _startValue = CalculateRotationDegree(Input.mousePosition);
                }

                if (Input.GetMouseButton(0))
                {
                    var currentDegree = CalculateRotationDegree(Input.mousePosition);
                    var delta = currentDegree - _startValue;
                    _levelPlaceTransform.Rotate(Vector3.forward, delta);
                    _startValue = currentDegree;
                }
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    _levelPlaceTransform ??= _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlace);
                    Touch t = Input.GetTouch(0);

                    if (t.phase == TouchPhase.Began)
                    {
                        _startValue = CalculateRotationDegree(t.position);
                    }

                    if (t.phase == TouchPhase.Moved)
                    {
                        var currentDegree = CalculateRotationDegree(t.position);
                        var delta = currentDegree - _startValue;
                        _levelPlaceTransform.Rotate(Vector3.forward, delta);
                        _startValue = currentDegree;
                    }
                }
            }
        }

        private float CalculateRotationDegree(Vector3 touchPosition)
        {
            _levelPlaceTransform ??= _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.LevelPlace);

            Vector3 movePos = new Vector3(touchPosition.x, touchPosition.y, 0);
            var objectPos = _mainCamera.WorldToScreenPoint(_levelPlaceTransform.position);
            objectPos.z = 0;
            var movement = movePos - objectPos;
            var target = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            return target;
        }
    }
}