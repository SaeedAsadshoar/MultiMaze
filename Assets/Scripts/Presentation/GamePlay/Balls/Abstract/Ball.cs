using Domain.Constants;
using Domain.Enum;
using Domain.GameEvents;
using Domain.Interface;
using Presentation.GamePlay.Balls.Interface;
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
        private bool _isInsideCup;

        private Transform _ballFreeZonePlace;
        private Transform _ballInPuzzlePlace;

        private IEventService _eventService;
        private IInGameRepositoryService _inGameRepositoryService;

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

        [Inject]
        private void Init(IEventService eventService,
            IInGameRepositoryService inGameRepositoryService)
        {
            _eventService = eventService;
            _inGameRepositoryService = inGameRepositoryService;
        }

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool memoryPool)
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
        }

        public virtual void Kill()
        {
            BackToPool();
        }

        public virtual void MoveInsideCup()
        {
            _isInsideCup = true;
            _eventService.Fire(GameEvents.ON_BALL_ENTERED_CUP, new OnBallEnteredCup());
        }

        public virtual void ExitPuzzle()
        {
            _ballFreeZonePlace ??= _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallFreeZonePlace);
            RootTransform.SetParent(_ballFreeZonePlace);
        }

        public void EnterPuzzle()
        {
            _ballInPuzzlePlace ??= _inGameRepositoryService.GetRepository((int)InGameRepositoryTypes.BallInPuzzlePlace);
            RootTransform.SetParent(_ballInPuzzlePlace);
        }

        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}