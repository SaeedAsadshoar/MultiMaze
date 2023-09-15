using System;
using DI.Pool;
using Domain.Enum;
using Domain.Interface;
using Services.FactorySystem.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services.FactorySystem.Service
{
    public class EnemyFactory : IEnemyFactory
    {
        //private Butcher1.Factory _butcher1Factory;

        private readonly DiContainer _diContainer;

        public EnemyFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public IFactoryObject GetEnemy(EnemyTypes enemyType)
        {
            try
            {
                /*switch (enemyType)
                {
                    case EnemyTypes.Butcher1:
                        return _butcher1Factory.Create();
                }*/

                return null;
            }
            catch (Exception e)
            {
                Debug.Log($"{enemyType} to zenject scene context --- error {e.Message}");
                return null;
            }
        }

        public void DefineFactory(EnemyTypes enemyType)
        {
            if (!IsFactoryLoaded(enemyType))
            {
                /*switch (enemyType)
                {
                    case EnemyTypes.Butcher1:
                        LoadFactory<Butcher1, Butcher1.Factory>(enemyType.ToString(), enemyType, _diContainer);
                        break;
                }*/
            }
        }

        public bool IsFactoryLoaded(EnemyTypes enemyType)
        {
            /*switch (enemyType)
            {
                case EnemyTypes.Butcher1:
                    return _butcher1Factory != null;
            }*/

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, EnemyTypes enemyType, DiContainer diContainer) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref diContainer, 0, "Enemies", handle.Result);

                /*switch (enemyType)
                {
                    case EnemyTypes.Butcher1:
                        _butcher1Factory = diContainer.Resolve<Butcher1.Factory>();
                        break;
                }*/
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}