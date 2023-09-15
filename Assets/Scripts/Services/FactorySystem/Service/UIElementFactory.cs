using System;
using System.Threading.Tasks;
using DI.Pool;
using Domain.Enum;
using Domain.Interface;
using Services.FactorySystem.Interface;
using Services.Toast.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Services.FactorySystem.Service
{
    public class UIElementFactory : IUIElementFactory
    {
        private readonly DiContainer _diContainer;

        private UIToast.Factory _uiToast;

        public UIElementFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public async Task LoadAllUIs()
        {
            var elementNames = Enum.GetNames(typeof(UiElementNames));
            int count = elementNames.Length;
            for (int i = 0; i < count; i++)
            {
                var elementName = (UiElementNames)i;
                DefineFactory(elementName);
                while (!IsFactoryLoaded(elementName))
                {
                    await Task.Delay(10);
                }
            }
        }

        public IFactoryObject GetUiElement(UiElementNames uiElementName)
        {
            switch (uiElementName)
            {
                case UiElementNames.UIToast:
                    return _uiToast.Create();
            }

            return null;
        }

        private void DefineFactory(UiElementNames uiElementName)
        {
            switch (uiElementName)
            {
                case UiElementNames.UIToast:
                    LoadFactory<UIToast, UIToast.Factory>(uiElementName.ToString(), uiElementName, _diContainer);
                    break;
            }
        }

        private bool IsFactoryLoaded(UiElementNames uiElementName)
        {
            switch (uiElementName)
            {
                case UiElementNames.UIToast:
                    return _uiToast != null;
            }

            return false;
        }

        private async void LoadFactory<T, TU>(string assetReference, UiElementNames uiElementName, DiContainer diContainer) where T : Component, IPoolable<IMemoryPool> where TU : PlaceholderFactory<T>
        {
            try
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetReference);
                await handle.Task;
                FactoryCreator<T, TU>.Create(ref diContainer, 0, "UIElements", handle.Result);

                switch (uiElementName)
                {
                    case UiElementNames.UIToast:
                        _uiToast = diContainer.Resolve<UIToast.Factory>();
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