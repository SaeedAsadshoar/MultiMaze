using System;
using System.Collections.Generic;
using System.Linq;
using Services.UpdateSystem.Data;
using Services.UpdateSystem.Enum;
using Services.UpdateSystem.Interface;
using UnityEngine;
using Zenject;

namespace Services.UpdateSystem.Service
{
    public class UnityUpdateService : IUnityUpdate, ITickable
    {
        private readonly Dictionary<string, ActionData> _invokeActions;
        private readonly List<Action> _updateFunctions;
        private bool _isActive;
        private int _counter = 1;

        public UnityUpdateService()
        {
            _invokeActions = new Dictionary<string, ActionData>();
            _updateFunctions = new List<Action>();
            _isActive = true;
        }

        public void Tick()
        {
            if (!_isActive) return;
            int lenght = _updateFunctions.Count;
            for (var index = 0; index < lenght; ++index)
            {
                var updateFunction = _updateFunctions[index];
                updateFunction?.Invoke();
            }

            var temp = _invokeActions.Values.ToList();
            lenght = temp.Count;

            for (var index = 0; index < lenght; ++index)
            {
                var actionData = temp[index];
                if (actionData.InvokeType == InvokeType.TimeBase)
                {
                    actionData.PassedTime += Time.deltaTime;
                    if (actionData.PassedTime > actionData.InvokeTime)
                    {
                        actionData.InvokeAction.Invoke();
                        _invokeActions.Remove(actionData.Id);
                    }
                }
                else
                {
                    actionData.InvokeTime++;
                    if (actionData.InvokeTime > 1)
                    {
                        actionData.InvokeAction.Invoke();
                        _invokeActions.Remove(actionData.Id);
                    }
                }
            }
        }

        public string Invoke(Action function, float time)
        {
            string id = DateTime.UtcNow.Ticks.ToString();
            if (_invokeActions.ContainsKey(id))
            {
                id = $"{id}_{_counter}";
                _counter++;
            }

            var actionData = new ActionData
            {
                Id = id,
                InvokeAction = function,
                PassedTime = 0,
                InvokeTime = time,
                InvokeType = InvokeType.TimeBase
            };

            _invokeActions.Add(id, actionData);
            return id;
        }

        public string DoInNextFrame(Action function)
        {
            string id = DateTime.UtcNow.Ticks.ToString();
            if (_invokeActions.ContainsKey(id))
            {
                id = $"{id}_{_counter}";
                _counter++;
            }

            var actionData = new ActionData
            {
                Id = id,
                InvokeAction = function,
                PassedTime = 0,
                InvokeTime = 0,
                InvokeType = InvokeType.NextFrame
            };

            _invokeActions.Add(id, actionData);
            return id;
        }

        public void CancelInvoke(string id)
        {
            _invokeActions.Remove(id);
        }

        public void AddToUpdate(Action updateFunction)
        {
            _updateFunctions.Add(updateFunction);
        }

        public void RemoveFromUpdate(Action updateFunction)
        {
            _updateFunctions.Remove(updateFunction);
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