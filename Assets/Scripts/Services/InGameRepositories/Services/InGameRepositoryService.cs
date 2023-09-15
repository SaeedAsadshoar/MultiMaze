using System.Collections.Generic;
using Services.InGameRepositories.Interface;
using UnityEngine;

namespace Services.InGameRepositories.Services
{
    public class InGameRepositoryService : IInGameRepositoryService
    {
        private readonly Dictionary<int, Transform> _repos;

        public InGameRepositoryService()
        {
            _repos = new Dictionary<int, Transform>();
        }

        public void AddRepository(int id, Transform repoTransform)
        {
            if (_repos.ContainsKey(id))
            {
                _repos[id] = repoTransform;
                return;
            }

            _repos.Add(id, repoTransform);
        }

        public Transform GetRepository(int id)
        {
            return _repos.TryGetValue(id, out var repository) ? repository : null;
        }
    }
}