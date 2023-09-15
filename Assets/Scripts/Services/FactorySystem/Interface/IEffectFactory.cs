using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IEffectFactory
    {
        void DefineFactory(FactoryEffectTypes effectType);
        IFactoryObject GetEffect(FactoryEffectTypes effectType);
        bool IsFactoryLoaded(FactoryEffectTypes effectType);
    }
}