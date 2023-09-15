using Domain.Enum;
using Domain.Interface;
using Services.FactorySystem.Interface;

namespace Services.FactorySystem.Service
{
    public class DataModelFactory : IDataModelFactory
    {
        //private readonly InventoryData.Factory _inventoryDataFactory;

        public DataModelFactory(/*InventoryData.Factory inventoryDataFactory*/)
        {
            //_inventoryDataFactory = inventoryDataFactory;
        }

        public IFactoryObject GetDataModel(FactoryDataModelTypes dataModelType)
        {
            /*switch (dataModelType)
            {
                case FactoryDataModelTypes.InventoryData:
                    return _inventoryDataFactory.Create();
            }*/

            return null;
        }
    }
}