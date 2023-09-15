using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Interface;
using Services.FactorySystem.Interface;
using Zenject;

namespace Services.FactorySystem.Service
{
    public class FactoryService : IFactoryService
    {
        private readonly IEffectFactory _effectFactory;
        private readonly IDataModelFactory _dataModelFactory;
        private readonly IEnemyFactory _enemyFactory;
        private readonly IUIElementFactory _uiElementFactory;
        private readonly IUIScreenFactory _uiScreenFactory;
        private readonly IPlayerFactory _playerFactory;
        private readonly DiContainer _diContainer;

        private List<EnemyTypes> _isLoadingEnemy;
        private List<HeroTypes> _isLoadingHero;
        private List<FactoryEffectTypes> _isLoadingEffect;

        public FactoryService(IEffectFactory effectFactory,
            IDataModelFactory dataModelFactory,
            IEnemyFactory enemyFactory,
            IUIElementFactory uiElementFactory,
            IPlayerFactory playerFactory,
            IUIScreenFactory uiScreenFactory,
            DiContainer diContainer)
        {
            _effectFactory = effectFactory;
            _dataModelFactory = dataModelFactory;
            _enemyFactory = enemyFactory;
            _uiElementFactory = uiElementFactory;
            _uiScreenFactory = uiScreenFactory;
            _playerFactory = playerFactory;
            _diContainer = diContainer;
            _isLoadingEnemy = new List<EnemyTypes>();
            _isLoadingHero = new List<HeroTypes>();
            _isLoadingEffect = new List<FactoryEffectTypes>();
        }

        public IFactoryObject GetDataModel(FactoryDataModelTypes dataModelType)
        {
            return _dataModelFactory.GetDataModel(dataModelType);
        }

        public IFactoryObject GetEffect(FactoryEffectTypes effectType)
        {
            if (!IsFactoryLoaded(effectType))
            {
                if (!_isLoadingEffect.Contains(effectType))
                {
                    _isLoadingEffect.Add(effectType);
                    DefineFactory(effectType);
                }

                return null;
            }

            _isLoadingEffect.Remove(effectType);
            return _effectFactory.GetEffect(effectType);
        }

        public IFactoryObject GetEnemy(EnemyTypes enemyType)
        {
            if (!IsFactoryLoaded(enemyType))
            {
                if (!_isLoadingEnemy.Contains(enemyType))
                {
                    _isLoadingEnemy.Add(enemyType);
                    DefineFactory(enemyType);
                }

                return null;
            }

            _isLoadingEnemy.Remove(enemyType);
            return _enemyFactory.GetEnemy(enemyType);
        }

        public IFactoryObject GetPlayer(HeroTypes heroType)
        {
            if (!IsFactoryLoaded(heroType))
            {
                if (!_isLoadingHero.Contains(heroType))
                {
                    _isLoadingHero.Add(heroType);
                    DefineFactory(heroType);
                }

                return null;
            }

            _isLoadingHero.Remove(heroType);
            return _playerFactory.GetPlayer(heroType);
        }

        public Task<IFactoryObject> GetUiElement(UiElementNames uiElementName)
        {
            return _uiElementFactory.GetUiElement(uiElementName);
        }

        public Task<IFactoryObject> GetUiPage(UiPanelNames panelName)
        {
            return _uiScreenFactory.GetUiScreen(panelName);
        }

        public bool IsFactoryLoaded(HeroTypes heroType)
        {
            return _playerFactory.IsFactoryLoaded(heroType);
        }

        public bool IsFactoryLoaded(EnemyTypes enemyType)
        {
            return _enemyFactory.IsFactoryLoaded(enemyType);
        }

        public bool IsFactoryLoaded(FactoryEffectTypes effectType)
        {
            return _effectFactory.IsFactoryLoaded(effectType);
        }

        public void DefineFactory(HeroTypes heroType)
        {
            if (IsFactoryLoaded(heroType))
            {
                _isLoadingHero.Remove(heroType);
                return;
            }

            if (!_isLoadingHero.Contains(heroType))
            {
                _playerFactory.DefineFactory(heroType);
            }
        }

        public void DefineFactory(EnemyTypes enemyType)
        {
            if (IsFactoryLoaded(enemyType))
            {
                _isLoadingEnemy.Remove(enemyType);
                return;
            }

            if (!_isLoadingEnemy.Contains(enemyType))
            {
                _enemyFactory.DefineFactory(enemyType);
            }
        }

        public void DefineFactory(FactoryEffectTypes effectType)
        {
            if (IsFactoryLoaded(effectType))
            {
                _isLoadingEffect.Remove(effectType);
                return;
            }

            if (!_isLoadingEffect.Contains(effectType))
            {
                _effectFactory.DefineFactory(effectType);
            }
        }
    }
}