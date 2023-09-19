using Domain.Data;
using Domain.Enum;
using UnityEngine;

namespace Services.ConfigService.Interface
{
    public interface IGameConfigService
    {
        ActionResult IsConfigLoaded { get; }
        TouchSettings TouchSetting { get; }
        float LoadProgress { get; }
        int MaxLevelCount { get; }
        bool IsLoadLevelByObjectPrefab { get; }
        void LoadGameConfig();
        LevelData GetLevelData(int levelIndex);
        BallPhysicsSettings GetBallPhysicsSetting(BallTypes ballType);
        ColorPalette GetRandomColorPalette();
    }
}