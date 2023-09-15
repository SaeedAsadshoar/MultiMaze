using UnityEngine;

namespace Services.InGameRepositories.Interface
{
    public interface IInGameRepositoryService
    {
        void AddRepository(int id, Transform repoTransform);
        Transform GetRepository(int id);
    }
}