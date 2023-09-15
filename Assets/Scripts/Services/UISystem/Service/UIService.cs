using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Constants;
using Domain.Enum;
using Domain.Events;
using Services.EventSystem.Interface;
using Services.FactorySystem.Interface;
using Services.UISystem.Interface;
using UnityEngine;

namespace Services.UISystem.Service
{
    public class UIService : IUIService
    {
        private readonly Transform _parent;
        private readonly Transform _parentLoading;
        private readonly Dictionary<UiPanelNames, IUIScreen> _pages;
        private readonly List<IUIScreen> _allPages;

        private readonly IFactoryService _factoryService;
        private readonly IEventService _eventService;

        public UIService(IFactoryService factoryService, IEventService eventService)
        {
            _factoryService = factoryService;
            _pages = new Dictionary<UiPanelNames, IUIScreen>();
            _allPages = new List<IUIScreen>();
            _parent = GameObject.FindGameObjectWithTag(TagNames.UI_PLACE).transform;
            _parentLoading = GameObject.FindGameObjectWithTag(TagNames.UI_LOADING_PLACE).transform;
            _eventService = eventService;
        }

        public IUIScreen OpenPage(UiPanelNames panelName, Action onOpenPage, Action onClose = null)
        {
            if (_pages.ContainsKey(panelName))
            {
                return null;
            }

            var page = _factoryService.GetUiPage(panelName) as IUIScreen;
            if (page == null) return null;
            page.SetParent(panelName == UiPanelNames.UILoading ? _parentLoading : _parent);

            page.Show(onClose);
            _eventService.Fire((int)GameEvents.OnPageOpen, new OnPageOpen(panelName));
            //page.SetParent(panelName == UiPanelNames.Loading ? _parentLoading : _parent);

            onOpenPage?.Invoke();
            _pages.Add(panelName, page);
            _allPages.Add(page);

            return page;
        }

        public void ClosePage(UiPanelNames panelName, Action onClosePage)
        {
            if (_pages.ContainsKey(panelName))
            {
                _allPages.Remove(_pages[panelName]);
                _pages[panelName].Close();
                _pages.Remove(panelName);
                onClosePage?.Invoke();
                _eventService.Fire((int)GameEvents.OnPageOpen, _allPages.Count > 0 ? new OnPageOpen(_allPages[^1].PanelName) : new OnPageOpen(UiPanelNames.UILoading));
            }
        }

        public void ClosePage(Action onClosePage)
        {
            var lastKey = _pages.Keys.Max();

            _pages[lastKey].Close();
            _pages.Remove(lastKey);
            onClosePage?.Invoke();
        }
    }
}