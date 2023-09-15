using System;
using System.Threading.Tasks;
using DI.Pool;
using Domain.Enum;
using Domain.Interface;
using Presentation.UI;
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

        private UILoading.Factory _uiLoadingFactory;

        public UIScreenFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAllUIs()
        {
            var allPanels = Enum.GetNames(typeof(UiPanelNames));
            int count = allPanels.Length;
            for (int i = 0; i < count; i++)
            {
                var panelName = (UiPanelNames)i;
                DefineFactory(panelName);
                while (!IsFactoryLoaded(panelName))
                {
                    await Task.Delay(10);
                }
            }
        }

        public IFactoryObject GetUiScreen(UiPanelNames panelName)
        {
            switch (panelName)
            {
                case UiPanelNames.UILoading:
                    return _uiLoadingFactory.Create();
            }

            return null;
        }

        private void DefineFactory(UiPanelNames panelName)
        {
            switch (panelName)
            {
                case UiPanelNames.UILoading:
                    LoadFactory<UILoading, UILoading.Factory>(panelName.ToString(), panelName, _diContainer);
                    break;
            }
        }

        private bool IsFactoryLoaded(UiPanelNames panelName)
        {
            switch (panelName)
            {
                case UiPanelNames.UILoading:
                    return _uiLoadingFactory != null;
            }

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, UiPanelNames panelName, DiContainer diContainer) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref diContainer, 0, "UIScreens", handle.Result);

                switch (panelName)
                {
                    case UiPanelNames.UILoading:
                        _uiLoadingFactory = diContainer.Resolve<UILoading.Factory>();
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