using System.Threading.Tasks;
using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IFactoryService
    {
        IFactoryObject GetDataModel(FactoryDataModelTypes dataModelType);
        IFactoryObject GetEffect(FactoryEffectTypes effectType);
        IFactoryObject GetEnemy(EnemyTypes enemyType);
        IFactoryObject GetPlayer(HeroTypes heroType);
        Task<IFactoryObject> GetUiElement(UiElementNames uiElementName);
        Task<IFactoryObject> GetUiPage(UiPanelNames panelName);
        void DefineFactory(HeroTypes heroType);
        void DefineFactory(EnemyTypes enemyType);
        void DefineFactory(FactoryEffectTypes effectType);
        bool IsFactoryLoaded(HeroTypes heroType);
        bool IsFactoryLoaded(EnemyTypes enemyType);
        bool IsFactoryLoaded(FactoryEffectTypes effectType);
    }
}