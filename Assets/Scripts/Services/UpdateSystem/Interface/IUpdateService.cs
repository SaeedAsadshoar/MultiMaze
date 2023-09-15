using System;
using Domain.Enum;

namespace Services.UpdateSystem.Interface
{
    public interface IUpdateService
    {
        /// <summary>
        /// perform an specific action on specific time
        /// </summary>
        /// <param name="function"></param>
        /// <param name="time"></param>
        /// <returns>id of invoke - store it if you want to cancel invoke</returns>
        string Invoke(Action function, float time);

        string DoInNextFrame(Action function);

        void CancelInvoke(string id);
        void AddToUpdate(Action updateFunction, UnityUpdateType updateType);
        void RemoveFromUpdate(Action updateFunction, UnityUpdateType updateType);
        void Start();
        void Stop();
    }
}