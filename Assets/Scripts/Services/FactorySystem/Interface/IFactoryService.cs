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
        IFactoryObject GetUiElement(UiElementNames uiElementName);
        IFactoryObject GetUiPage(UiPanelNames panelName);
        Task LoadUiElements();
        Task LoadUiPanels();
        void DefineFactory(HeroTypes heroType);
        void DefineFactory(EnemyTypes enemyType);
        void DefineFactory(FactoryEffectTypes effectType);
        bool IsFactoryLoaded(HeroTypes heroType);
        bool IsFactoryLoaded(EnemyTypes enemyType);
        bool IsFactoryLoaded(FactoryEffectTypes effectType);
    }
}