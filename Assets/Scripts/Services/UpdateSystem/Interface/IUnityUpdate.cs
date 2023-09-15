using System;

namespace Services.UpdateSystem.Interface
{
    public interface IUnityUpdate
    {
        string Invoke(Action function, float time);
        string DoInNextFrame(Action function);
        void CancelInvoke(string id);
        void AddToUpdate(Action updateFunction);
        void RemoveFromUpdate(Action updateFunction);
        void Start();
        void Stop();
    }
}