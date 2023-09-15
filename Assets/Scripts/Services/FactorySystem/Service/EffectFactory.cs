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
    public class EffectFactory : IEffectFactory
    {
        //private CanonBall.Factory _canonBallFactory;

        private readonly DiContainer _diContainer;

        public EffectFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void DefineFactory(FactoryEffectTypes effectType)
        {
            if (!IsFactoryLoaded(effectType))
            {
                /*switch (effectType)
                {
                    case FactoryEffectTypes.CanonBall:
                        LoadFactory<CanonBall, CanonBall.Factory>(effectType.ToString(), effectType, _diContainer);
                        break;
                }*/
            }
        }

        public IFactoryObject GetEffect(FactoryEffectTypes effectType)
        {
            /*switch (effectType)
            {
                case FactoryEffectTypes.CanonBall:
                    return _canonBallFactory.Create();
            }*/

            return null;
        }

        public bool IsFactoryLoaded(FactoryEffectTypes effectType)
        {
            /*switch (effectType)
            {
                case FactoryEffectTypes.CanonBall:
                    return _canonBallFactory != null;
            }*/

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, FactoryEffectTypes effectType, DiContainer diContainer) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref diContainer, 0, "Effects", handle.Result);

                /*switch (effectType)
                {
                    case FactoryEffectTypes.CanonBall:
                        _canonBallFactory = diContainer.Resolve<CanonBall.Factory>();
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