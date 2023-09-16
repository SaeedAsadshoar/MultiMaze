using Domain.Data;
using UnityEngine;

namespace Services.ConfigService.Interface
{
    public interface IGameConfigService
    {
        ActionResult IsConfigLoaded { get; }
        float LoadProgress { get; }
        int MaxLevelCount { get; }
        void LoadGameConfig();
        LevelData GetLevelData(int levelIndex);
    }
}