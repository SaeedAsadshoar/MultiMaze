using Domain.Data;

namespace Services.ConfigService.Interface
{
    public interface IGameConfigService
    {
        ActionResult IsConfigLoaded { get; }
        float LoadProgress { get; }
        void LoadGameConfig();
    }
}