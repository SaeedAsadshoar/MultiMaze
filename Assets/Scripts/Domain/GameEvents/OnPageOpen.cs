using Domain.Enum;

namespace Domain.GameEvents
{
    public class OnPageOpen
    {
        public UiPanelNames PanelName { get; }

        public OnPageOpen(UiPanelNames panelName)
        {
            PanelName = panelName;
        }
    }
}