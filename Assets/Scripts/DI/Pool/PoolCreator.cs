using UnityEngine;
using Zenject;

namespace DI.Pool
{
    public class PoolCreator<T> : MonoPoolableMemoryPool<IMemoryPool, T> where T : Component, IPoolable<IMemoryPool>
    {
    }
}