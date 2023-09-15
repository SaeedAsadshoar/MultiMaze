using System;
using System.Collections.Generic;
using Services.UpdateSystem.Interface;
using Zenject;

namespace Services.UpdateSystem.Service
{
    public class UnityFixedUpdateService : IUnityFixedUpdate, IFixedTickable
    {
        private readonly List<Action> _fixedUpdateFunctions;
        private bool _isActive;

        public UnityFixedUpdateService()
        {
            _fixedUpdateFunctions = new List<Action>();
            _isActive = true;
        }
        
        public void FixedTick()
        {
            if (!_isActive) return;
            int lenght = _fixedUpdateFunctions.Count;
            for (var index = 0; index < lenght; ++index)
            {
                var updateFunction = _fixedUpdateFunctions[index];
                updateFunction?.Invoke();
            }
        }

        public void AddToUpdate(Action updateFunction)
        {
            _fixedUpdateFunctions.Add(updateFunction);
        }

        public void RemoveFromUpdate(Action updateFunction)
        {
            _fixedUpdateFunctions.Remove(updateFunction);
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