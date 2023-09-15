using System;
using Services.UpdateSystem.Enum;

namespace Services.UpdateSystem.Data
{
    public class ActionData
    {
        public string Id;
        public InvokeType InvokeType;
        public Action InvokeAction;
        public float PassedTime;
        public float InvokeTime;
    }
    
    public class ActionData<T>
    {
        public string Id;
        public InvokeType InvokeType;
        public Action<T> InvokeAction;
        public float PassedTime;
        public float InvokeTime;
    }
}