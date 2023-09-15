using Zenject;

namespace Services.MemoryPoolSystem.Interface
{
    public interface IMemoryPoolService
    {
        void AddMemoryPool(IMemoryPool memoryPool);
        void ClearAllPool();
        void ClearMemoryPool(IMemoryPool memoryPool);
    }
}