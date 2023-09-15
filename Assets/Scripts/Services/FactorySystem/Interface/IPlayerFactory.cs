using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IPlayerFactory
    {
        IFactoryObject GetPlayer(HeroTypes heroType);
        void DefineFactory(HeroTypes heroType);
        bool IsFactoryLoaded(HeroTypes heroType);
    }
}