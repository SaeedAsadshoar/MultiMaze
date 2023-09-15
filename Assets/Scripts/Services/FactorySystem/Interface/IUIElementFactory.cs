using System.Threading.Tasks;
using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IUIElementFactory
    {
        Task LoadAllUIs();
        IFactoryObject GetUiElement(UiElementNames uiElementName);
    }
}