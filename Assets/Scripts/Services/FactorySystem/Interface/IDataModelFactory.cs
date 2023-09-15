using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IDataModelFactory
    {
        IFactoryObject GetDataModel(FactoryDataModelTypes dataModelType);
    }
}