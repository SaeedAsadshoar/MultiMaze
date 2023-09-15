using UnityEngine;
using Zenject;

namespace DI.Pool
{
    public class FactoryCreator<T, TU> where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
    {
        public static void Create(ref DiContainer container, int initialCount, string rootName, GameObject prefab)
        {
            container.BindFactory<T, TU>()
                .FromPoolableMemoryPool<T, PoolCreator<T>>(poolBinder => poolBinder
                    .WithInitialSize(initialCount)
                    .FromComponentInNewPrefab(prefab)
                    .UnderTransformGroup(rootName)).Lazy();
        }
    }

    public class FactoryCreatorNonPool<T, TU> where T : Component where TU : PlaceholderFactory<T>
    {
        public static void Create(ref DiContainer container, int initialCount, string rootName, GameObject prefab)
        {
            container.BindFactory<T, TU>().FromComponentInNewPrefab(prefab).UnderTransformGroup(rootName).Lazy();
        }
    }
}