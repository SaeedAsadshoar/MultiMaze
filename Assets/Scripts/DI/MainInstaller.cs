using Services.AudioSystem.Interface;
using Services.AudioSystem.Service;
using Services.ConfigService.Interface;
using Services.ConfigService.Service;
using Services.EventSystem.Interface;
using Services.EventSystem.Service;
using Services.FactorySystem.Interface;
using Services.FactorySystem.Service;
using Services.InGameRepositories.Interface;
using Services.InGameRepositories.Services;
using Services.LevelLoader.Interface;
using Services.LevelLoader.Service;
using Services.MemoryPoolSystem.Interface;
using Services.MemoryPoolSystem.Service;
using Services.StorageSystem.Interface;
using Services.StorageSystem.Service;
using Services.Toast.Interface;
using Services.Toast.Service;
using Services.UISystem.Interface;
using Services.UISystem.Service;
using Services.UpdateSystem.Interface;
using Services.UpdateSystem.Service;
using UnityEngine;
using Zenject;

namespace DI
{
    [CreateAssetMenu(fileName = "MainInstaller", menuName = "Installers/MainInstaller")]
    public class MainInstaller : ScriptableObjectInstaller<MainInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IGameConfigService>().To<GameConfigService>().AsSingle().NonLazy();

            Container.Bind<IEventService>().To<EventService>().AsSingle().NonLazy();
            Container.Bind<IStorageService>().To<StorageService>().AsSingle().NonLazy();
            Container.Bind<IUIService>().To<UIService>().AsSingle().NonLazy();
            Container.Bind<ILevelLoaderService>().To<LevelLoaderService>().AsSingle().NonLazy();

            Container.Bind<IFactoryService>().To<FactoryService>().AsSingle().NonLazy();
            Container.Bind<IDataModelFactory>().To<DataModelFactory>().AsSingle().NonLazy();
            Container.Bind<IUIElementFactory>().To<UIElementFactory>().AsSingle().NonLazy();
            Container.Bind<IUIScreenFactory>().To<UIScreenFactory>().AsSingle().NonLazy();
            Container.Bind<IBallFactory>().To<BallFactory>().AsSingle().NonLazy();

            Container.Bind<IInGameRepositoryService>().To<InGameRepositoryService>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<UnityUpdateService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UnityFixedUpdateService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<UnityLateUpdateService>().AsSingle().NonLazy();

            Container.Bind<IUpdateService>().To<UpdateService>().AsSingle().NonLazy();

            Container.Bind<IToastService>().To<ToastService>().AsSingle().NonLazy();
            Container.Bind<IMemoryPoolService>().To<MemoryPoolService>().AsSingle().NonLazy();
            Container.Bind<IAudioService>().To<AudioService>().AsSingle().NonLazy();
        }
    }
}