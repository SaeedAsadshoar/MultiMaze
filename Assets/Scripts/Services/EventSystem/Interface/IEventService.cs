using System;

namespace Services.EventSystem.Interface
{
    public interface IEventService
    {
        void Subscribe<T>(int eventId, Action<T> eventClass);
        void RemoveEvent<T>(int eventId, Action<T> eventClass);
        void Fire<T>(int eventId, T eventClass);
    }
}