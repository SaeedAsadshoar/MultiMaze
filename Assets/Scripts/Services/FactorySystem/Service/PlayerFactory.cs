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
    public class PlayerFactory : IPlayerFactory
    {
        //private MageFactory.Factory _mageFactory;

        private readonly DiContainer _diContainer;

        public PlayerFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public void DefineFactory(HeroTypes heroType)
        {
            //_diContainer.Unbind<MageFactory.Factory>();

            /*switch (heroType)
            {
                case HeroTypes.Mage:
                    if (_mageFactory == null)
                        LoadFactory<MageFactory, MageFactory.Factory>(heroType.ToString(), heroType, _diContainer);
                    break;
            }*/

            Resources.UnloadUnusedAssets();
        }

        public bool IsFactoryLoaded(HeroTypes heroType)
        {
            /*switch (heroType)
            {
                case HeroTypes.Mage:
                    return _mageFactory != null;
            }*/

            return false;
        }

        public IFactoryObject GetPlayer(HeroTypes heroType)
        {
            switch (heroType)
            {
                /*case HeroTypes.Mage:
                    return _mageFactory.Create();*/
                default:
                    return null;
            }
        }

        private async void LoadFactory<T, TU>(string assetReference, HeroTypes heroType, DiContainer container) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref container, 0, "Player", handle.Result);

                /*switch (heroType)
                {
                    case HeroTypes.Mage:
                        _mageFactory = container.Resolve<MageFactory.Factory>();
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