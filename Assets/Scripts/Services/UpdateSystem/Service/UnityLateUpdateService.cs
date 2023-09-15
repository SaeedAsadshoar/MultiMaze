using System;
using System.Collections.Generic;
using Services.UpdateSystem.Interface;
using Zenject;

namespace Services.UpdateSystem.Service
{
    public class UnityLateUpdateService : IUnityLateUpdate,ILateTickable
    {
        private readonly List<Action> _lateUpdateFunctions;
        private bool _isActive;
        
        public UnityLateUpdateService()
        {
            _lateUpdateFunctions = new List<Action>();
            _isActive = true;
        }
        
        public void LateTick()
        {
            if (!_isActive) return;
            int lenght = _lateUpdateFunctions.Count;
            for (var index = 0; index < lenght; ++index)
            {
                var updateFunction = _lateUpdateFunctions[index];
                updateFunction?.Invoke();
            }
        }

        public void AddToUpdate(Action updateFunction)
        {
            _lateUpdateFunctions.Add(updateFunction);
        }

        public void RemoveFromUpdate(Action updateFunction)
        {
            _lateUpdateFunctions.Remove(updateFunction);
        }

        public void Start()
        {
            _isActive = true;
        }

        public void Stop()
        {
            _isActive = false;
        }
    }
}