using Domain.Data;

namespace Services.Config.Interface
{
    public interface IGameConfigService
    {
        ActionResult IsConfigLoaded { get; }
    }
}