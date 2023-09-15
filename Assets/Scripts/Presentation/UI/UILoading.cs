using System;
using Domain.Enum;
using Services.UISystem.Abstract;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Presentation.UI
{
    public class UILoading : EachUIPanel<UILoading>
    {
        [SerializeField] private TextMeshProUGUI _loadingNameLabel;
        [SerializeField] private Image _loadingProgressImage;
        public override UiPanelNames PanelName => UiPanelNames.UILoading;

        private void OnEnable()
        {
            _loadingNameLabel.text = string.Empty;
            _loadingProgressImage.fillAmount = 0;
        }

        public void SetLoading(string loadName)
        {
            _loadingNameLabel.text = loadName;
        }

        public void SetProgress(float progress)
        {
            _loadingProgressImage.fillAmount = progress;
        }
    }
}