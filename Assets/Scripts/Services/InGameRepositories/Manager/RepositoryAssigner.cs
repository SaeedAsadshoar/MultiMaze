using Domain.Enum;
using Services.InGameRepositories.Interface;
using UnityEngine;
using Zenject;

namespace Services.InGameRepositories.Manager
{
    public class RepositoryAssigner : MonoBehaviour
    {
        [SerializeField] private InGameRepositoryTypes _inGameRepositoryType;

        [Inject]
        private void Init(IInGameRepositoryService inGameRepositoryService)
        {
            inGameRepositoryService.AddRepository((int)_inGameRepositoryType, transform);
        }
    }
}