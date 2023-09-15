using Domain.Interface;
using UnityEngine;
using Zenject;

namespace Services.UISystem.Abstract
{
    public class EachUIElement<T> : MonoBehaviour, IPoolable<IMemoryPool>, IFactoryObject, IPoolObject
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

        public virtual void OnDespawned()
        {
        }

        public virtual void OnSpawned(IMemoryPool memoryPool)
        {
            _memoryPool = memoryPool;
        }

        public virtual void BackToPool()
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