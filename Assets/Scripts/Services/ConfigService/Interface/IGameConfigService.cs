using Domain.Data;
using UnityEngine;

namespace Services.ConfigService.Interface
{
    public interface IGameConfigService
    {
        ActionResult IsConfigLoaded { get; }
        float LoadProgress { get; }
        void LoadGameConfig();
        GameObject GetLevelObject(int levelIndex);
        GameObject GetLevelCup(int levelIndex);
    }
}