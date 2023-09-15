using System;
using Domain.Enum;
using UnityEngine;

namespace Services.UISystem.Interface
{
    public interface  IUIScreen
    {
        UiPanelNames PanelName { get; }
        bool IsClosed { get; set; }
        void Show(Action onClose = null);
        void Close();
        void SetParent(Transform parent);
    }
}