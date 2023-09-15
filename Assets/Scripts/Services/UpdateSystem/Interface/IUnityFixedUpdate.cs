using System;

namespace Services.UpdateSystem.Interface
{
    public interface IUnityFixedUpdate
    {
        void AddToUpdate(Action updateFunction);
        void RemoveFromUpdate(Action updateFunction);
        void Start();
        void Stop();
    }
}