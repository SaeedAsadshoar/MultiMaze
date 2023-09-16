using Domain.Interface;
using Presentation.GamePlay.Balls.Interface;
using UnityEngine;
using Zenject;

namespace Presentation.GamePlay.Balls.Abstract
{
    public class Ball<T> : MonoBehaviour, IPoolable<IMemoryPool>, IFactoryObject, IPoolObject, IBall
    {
        private Transform _transform;
        private IMemoryPool _memoryPool;

        public Transform ObjectRoot
        {
            get
            {
                if (_transform == null) _transform = transform;
                return _transform;
            }
        }

        public IMemoryPool MemoryPool => _memoryPool;

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
        
        public class Factory : PlaceholderFactory<T>
        {
        }
    }
}