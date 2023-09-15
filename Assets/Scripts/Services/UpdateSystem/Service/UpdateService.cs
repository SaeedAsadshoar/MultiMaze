using System;
using Domain.Enum;
using Services.UpdateSystem.Interface;

namespace Services.UpdateSystem.Service
{
    public class UpdateService : IUpdateService
    {
        private readonly IUnityUpdate _unityUpdate;
        private readonly IUnityFixedUpdate _unityFixedUpdate;
        private readonly IUnityLateUpdate _unityLateUpdate;

        public UpdateService(IUnityUpdate unityUpdate,
            IUnityFixedUpdate unityFixedUpdate,
            IUnityLateUpdate unityLateUpdate)
        {
            _unityUpdate = unityUpdate;
            _unityFixedUpdate = unityFixedUpdate;
            _unityLateUpdate = unityLateUpdate;
        }

        string IUpdateService.Invoke(Action function, float time)
        {
            return _unityUpdate.Invoke(function, time);
        }

        string IUpdateService.DoInNextFrame(Action function)
        {
            return _unityUpdate.DoInNextFrame(function);
        }

        void IUpdateService.CancelInvoke(string id)
        {
            _unityUpdate.CancelInvoke(id);
        }

        public void AddToUpdate(Action updateFunction, UnityUpdateType updateType)
        {
            switch (updateType)
            {
                case UnityUpdateType.Update:
                    _unityUpdate.AddToUpdate(updateFunction);
                    break;
                case UnityUpdateType.FixedUpdate:
                    _unityFixedUpdate.AddToUpdate(updateFunction);
                    break;
                case UnityUpdateType.LateUpdate:
                    _unityLateUpdate.AddToUpdate(updateFunction);
                    break;
            }
        }

        public void RemoveFromUpdate(Action updateFunction, UnityUpdateType updateType)
        {
            switch (updateType)
            {
                case UnityUpdateType.Update:
                    _unityUpdate.RemoveFromUpdate(updateFunction);
                    break;
                case UnityUpdateType.FixedUpdate:
                    _unityFixedUpdate.RemoveFromUpdate(updateFunction);
                    break;
                case UnityUpdateType.LateUpdate:
                    _unityLateUpdate.RemoveFromUpdate(updateFunction);
                    break;
            }
        }

        public void Start()
        {
            _unityUpdate.Start();
            _unityFixedUpdate.Start();
            _unityLateUpdate.Start();
        }

        public void Stop()
        {
            _unityUpdate.Stop();
            _unityFixedUpdate.Stop();
            _unityLateUpdate.Stop();
        }
    }
}