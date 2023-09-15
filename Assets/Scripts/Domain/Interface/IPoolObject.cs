using UnityEngine;
using Zenject;

namespace Domain.Interface
{
    public interface IPoolObject
    {
        Transform ObjectRoot { get; }
        IMemoryPool MemoryPool { get; }
        void BackToPool();
    }
}