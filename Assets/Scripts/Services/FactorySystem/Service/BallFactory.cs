using System;
using System.Threading.Tasks;
using DI.Pool;
using Domain.Enum;
using Domain.Interface;
using Presentation.GamePlay.Balls;
using Services.FactorySystem.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services.FactorySystem.Service
{
    public class BallFactory : IBallFactory
    {
        private readonly DiContainer _diContainer;

        private SimpleBall.Factory _simpleBallFactory;

        public BallFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAllBalls()
        {
            var allBalls = Enum.GetNames(typeof(BallTypes));
            int count = allBalls.Length;
            for (int i = 0; i < count; i++)
            {
                var ballTypes = (BallTypes)i;
                DefineFactory(ballTypes);
                while (!IsFactoryLoaded(ballTypes))
                {
                    await Task.Delay(10);
                }
            }
        }

        public IFactoryObject GetBall(BallTypes ballType)
        {
            switch (ballType)
            {
                case BallTypes.SimpleBall:
                    return _simpleBallFactory.Create();
                default:
                    return null;
            }
        }

        public void DefineFactory(BallTypes ballType)
        {
            switch (ballType)
            {
                case BallTypes.SimpleBall:
                    if (_simpleBallFactory == null)
                        LoadFactory<SimpleBall, SimpleBall.Factory>(ballType.ToString(), ballType, _diContainer);
                    break;
            }

            Resources.UnloadUnusedAssets();
        }

        public bool IsFactoryLoaded(BallTypes ballType)
        {
            switch (ballType)
            {
                case BallTypes.SimpleBall:
                    return _simpleBallFactory != null;
            }

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, BallTypes ballType, DiContainer container) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref container, 0, "Balls", handle.Result);

                switch (ballType)
                {
                    case BallTypes.SimpleBall:
                        _simpleBallFactory = container.Resolve<SimpleBall.Factory>();
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}