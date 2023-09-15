using System;
using System.Collections.Generic;
using Services.MemoryPoolSystem.Interface;
using UnityEngine;
using Zenject;

namespace Services.MemoryPoolSystem.Service
{
    public class MemoryPoolService : IMemoryPoolService
    {
        private readonly List<IMemoryPool> _memoryPools;

        public MemoryPoolService()
        {
            _memoryPools = new List<IMemoryPool>();
        }

        public void AddMemoryPool(IMemoryPool memoryPool)
        {
            if (!_memoryPools.Contains(memoryPool))
            {
                _memoryPools.Add(memoryPool);
            }
        }

        public void ClearAllPool()
        {
            foreach (var memoryPool in _memoryPools)
            {
                try
                {
                    if (memoryPool != null)
                        memoryPool.Clear();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }

            _memoryPools.Clear();
        }

        public void ClearMemoryPool(IMemoryPool memoryPool)
        {
            memoryPool.Clear();
            _memoryPools.Remove(memoryPool);
        }
    }
}