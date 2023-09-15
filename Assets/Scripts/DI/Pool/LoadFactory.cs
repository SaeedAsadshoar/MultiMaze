using Domain.Enum;
using Services.FactorySystem.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace DI.Pool
{
    public static class LoadFactory
    {
        public static void Load<T, TU>(AssetReference assetReference, EnemyTypes enemyType, DiContainer container, int initialCount = 0, string parentName = "pool")
            where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            container.BindAsync<GameObject>().FromMethod(async () =>
            {
                try
                {
                    AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                    await handle.Task;
                    var diContainer = container;
                    FactoryCreator<T, TU>.Create(ref diContainer, initialCount, parentName, handle.Result);
                    IEnemyFactory enemyFactory = diContainer.Resolve<IEnemyFactory>();
                    enemyFactory.DefineFactory(enemyType);
                    return handle.Result;
                }
                catch (InvalidKeyException ex)
                {
                    Debug.Log(ex.Message);
                }

                return null;
            }).AsCached().NonLazy();
        }
        
        public static void LoadNonPool<T, TU>(AssetReference assetReference, EnemyTypes enemyType, DiContainer container, int initialCount = 0, string parentName = "pool")
            where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            container.BindAsync<GameObject>().FromMethod(async () =>
            {
                try
                {
                    AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                    await handle.Task;
                    var diContainer = container;
                    FactoryCreatorNonPool<T, TU>.Create(ref diContainer, initialCount, parentName, handle.Result);
                    IEnemyFactory enemyFactory = diContainer.Resolve<IEnemyFactory>();
                    enemyFactory.DefineFactory(enemyType);
                    return handle.Result;
                }
                catch (InvalidKeyException ex)
                {
                    Debug.Log(ex.Message);
                }

                return null;
            }).AsCached().NonLazy();
        }
    }
}