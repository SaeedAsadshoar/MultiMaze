using System.Threading.Tasks;
using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IUIElementFactory
    {
        Task<IFactoryObject> GetUiElement(UiElementNames uiElementName);
        void DefineFactory(UiElementNames uiElementName);
        bool IsFactoryLoaded(UiElementNames uiElementName);
    }
}