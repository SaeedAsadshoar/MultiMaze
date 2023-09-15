using System;
using System.Threading.Tasks;
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
    public class UIScreenFactory : IUIScreenFactory
    {
        private readonly DiContainer _diContainer;

        public UIScreenFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task<IFactoryObject> GetUiScreen(UiPanelNames panelName)
        {
            if (!IsFactoryLoaded(panelName)) DefineFactory(panelName);

            while (!IsFactoryLoaded(panelName))
            {
                await Task.Delay(10);
            }

            switch (panelName)
            {
                /*case UiPanelNames.UIGame:
                    return _uiGame.Create();*/
            }

            return null;
        }
        
        public void DefineFactory(UiPanelNames panelName)
        {
            switch (panelName)
            {
                /*case UiPanelNames.UIGame:
                    LoadFactory<UIGame, UIGame.Factory>(panelName.ToString(), panelName, _diContainer);
                    break;*/
            }
        }

        public bool IsFactoryLoaded(UiPanelNames panelName)
        {
            switch (panelName)
            {
                /*case UiPanelNames.UIGame:
                    return _uiGame != null;*/
            }

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, UiPanelNames panelName, DiContainer diContainer) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref diContainer, 0, "UIElements", handle.Result);

                switch (panelName)
                {
                    /*case UiPanelNames.UIGame:
                        _uiGame = diContainer.Resolve<UIGame.Factory>();
                        break;*/
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }
}