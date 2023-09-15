using Domain.Enum;
using Domain.Interface;

namespace Services.FactorySystem.Interface
{
    public interface IEnemyFactory
    {
        IFactoryObject GetEnemy(EnemyTypes enemyType);
        void DefineFactory(EnemyTypes enemyType);
        bool IsFactoryLoaded(EnemyTypes enemyType);
    }
}