using Domain.Enum;

namespace Domain.Events
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