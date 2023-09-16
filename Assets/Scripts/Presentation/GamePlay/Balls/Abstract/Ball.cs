using Domain.Interface;
using Presentation.GamePlay.Balls.Interface;
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

        public Transform ObjectRoot
        {
            get
            {
                if (_transform == null) _transform = transform;
                return _transform;
            }
        }

        public IMemoryPool MemoryPool => _memoryPool;
        public Transform RootTransform => ObjectRoot;

        public Rigidbody ObjectRigidbody
        {
            get
            {
                if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        public void OnDespawned()
        {
        }

        public void OnSpawned(IMemoryPool memoryPool)
        {
            _memoryPool = memoryPool;
        }

        public void BackToPool()
        {
            if (_memoryPool == null) return;
            _memoryPool.Despawn(this);
            _memoryPool = null;
        }

        public void Kill()
        {
            BackToPool();
        }

        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}