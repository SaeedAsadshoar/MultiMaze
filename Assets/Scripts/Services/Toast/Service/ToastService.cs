using System;
using System.Collections.Generic;
using Domain.Constants;
using Domain.Enum;
using Services.FactorySystem.Interface;
using Services.Toast.Interface;
using Services.Toast.UI;
using Services.UpdateSystem.Interface;
using UnityEngine;

namespace Services.Toast.Service
{
    public class ToastService : IToastService
    {
        private readonly IFactoryService _factoryService;
        private readonly Transform _toastParent;

        private float _toastTime = 3;
        private float _startShowToastTime;
        private bool _showToast;

        private UIToast _currentToast;
        private readonly Queue<EachToastMessage> _uiToasts;

        public ToastService(IFactoryService factoryService,
            IUpdateService updateService)
        {
            _uiToasts = new Queue<EachToastMessage>();
            _factoryService = factoryService;
            _toastParent = GameObject.FindGameObjectWithTag(TagNames.TOAST_PANEL).transform;
            updateService.AddToUpdate(UpdateFunction, UnityUpdateType.Update);
        }

        public void ShowToast(string title, string message, Sprite thumb)
        {
            if (_uiToasts.Count > 0)
            {
                if (string.Compare(_uiToasts.Peek().Title, title, StringComparison.Ordinal) == 0)
                    return;
            }

            if (_currentToast != null)
            {
                if (string.Compare(_currentToast.Message.Title, title, StringComparison.Ordinal) == 0)
                    return;
            }

            _uiToasts.Enqueue(new EachToastMessage() { Message = message, Title = title, Thumb = thumb });
        }

        private void UpdateFunction()
        {
            if (_showToast)
            {
                _startShowToastTime += Time.deltaTime;
                if (_startShowToastTime > _toastTime)
                {
                    _currentToast.Close();
                    _currentToast = null;
                    _showToast = false;
                }
            }
            else
            {
                if (_uiToasts.Count > 0)
                {
                    ShowToast(_uiToasts.Dequeue());
                }
            }
        }

        private async void ShowToast(EachToastMessage toastMessage)
        {
            var task = _factoryService.GetUiElement(UiElementNames.Toast);
            await task;
            var toast = task.Result as UIToast;
            if (toast != null)
            {
                toast.Show(toastMessage, _toastParent);
                _currentToast = toast;
            }

            _showToast = true;
            _startShowToastTime = 0;
        }

        public struct EachToastMessage
        {
            public string Message;
            public string Title;
            public Sprite Thumb;
        }
    }
}