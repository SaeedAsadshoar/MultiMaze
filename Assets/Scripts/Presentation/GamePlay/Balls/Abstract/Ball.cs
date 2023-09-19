using Domain.Constants;
using Domain.Data;
using Domain.Enum;
using Domain.GameEvents;
using Domain.Interface;
using Presentation.GamePlay.Balls.Interface;
using Services.ConfigService.Interface;
using Services.EventSystem.Interface;
using Services.InGameRepositories.Interface;
using UnityEngine;
using Zenject;

namespace Presentation.GamePlay.Balls.Abstract
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball<T> : MonoBehaviour, IPoolable<IMemoryPool>, IFactoryObject, IPoolObject, IBall
    {
        private Transform _transform;
        private IMemoryPool _memoryPool;
        private Rigidbody _rigidbody;
        private PhysicMaterial _physicMaterial;
        private bool _isInsideCup;

        private Transform _ballFreeZonePlace;
        private Transform _ballInPuzzlePlace;

        protected IEventService EventService;
        protected IInGameRepositoryService InGameRepositoryService;
        protected IGameConfigService GameConfigService;

        public IMemoryPool MemoryPool => _memoryPool;
        public Transform RootTransform => ObjectRoot;
        public bool IsInsideCup => _isInsideCup;

        public virtual Transform ObjectRoot
        {
            get
            {
                if (_transform == null) _transform = transform;
                return _transform;
            }
        }

        public virtual Rigidbody ObjectRigidbody
        {
            get
            {
                if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        public virtual PhysicMaterial PhysicMaterial
        {
            get
            {
                if (_physicMaterial == null) _physicMaterial = GetComponent<Collider>().material;
                return _physicMaterial;
            }
        }

        [Inject]
        private void Init(IEventService eventService,
            IInGameRepositoryService inGameRepositoryService,
            IGameConfigService gameConfigService)
        {
            EventService = eventService;
            InGameRepositoryService = inGameRepositoryService;
            GameConfigService = gameConfigService;
        }

        public void OnDespawned()
        {
        }

        public virtual void OnSpawned(IMemoryPool memoryPool)
        {
            _isInsideCup = false;
            _memoryPool = memoryPool;
        }

        public void BackToPool()
        {
            if (_memoryPool == null) return;
            _memoryPool.Despawn(this);
            _memoryPool = null;
            _isInsideCup = false;
            ObjectRigidbody.velocity = Vector3.zero;
        }

        public virtual void Kill()
        {
            BackToPool();
        }

        public virtual void MoveInsideCup()
        {
            _isInsideCup = true;
            EventService.Fire(GameEvents.ON_BALL_ENTERED_CUP, new OnBallEnteredCup());
        }

        public virtual void ExitPuzzle()
        {
            _ballFreeZonePlace ??= InGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallFreeZonePlace);
            _ballInPuzzlePlace ??= InGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallInPuzzlePlace);

            if (RootTransform.parent == _ballInPuzzlePlace)
            {
                RootTransform.SetParent(_ballFreeZonePlace);
                EventService.Fire(GameEvents.ON_BALL_EXIT_PUZZLE, new OnBallExitPuzzle());
            }
        }

        public void EnterPuzzle()
        {
            _ballFreeZonePlace ??= InGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallFreeZonePlace);
            _ballInPuzzlePlace ??= InGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallInPuzzlePlace);

            if (RootTransform.parent == _ballFreeZonePlace)
            {
                RootTransform.SetParent(_ballInPuzzlePlace);
                EventService.Fire(GameEvents.ON_BALL_ENTERED_PUZZLE, new OnBallEnteredPuzzle());
            }
        }

        public virtual void SetColorPalette(ColorPalette colorPalette)
        {
            throw new System.NotImplementedException();
        }

        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}