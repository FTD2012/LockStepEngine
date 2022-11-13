using System;

namespace LockStepEngine
{
    public interface IEventRegisterService : IService
    {
        void RegisterEvent(object obj);
        void UnRegisterEvent(object obj);
        void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen, Action<TEnum, TDelegate> callback, object obj) where TEnum : struct where TDelegate : Delegate;
    }
}