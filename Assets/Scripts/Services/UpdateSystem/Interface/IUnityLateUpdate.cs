using System;

namespace Services.UpdateSystem.Interface
{
    public interface IUnityLateUpdate
    {
        void AddToUpdate(Action updateFunction);
        void RemoveFromUpdate(Action updateFunction);
        void Start();
        void Stop();
    }
}