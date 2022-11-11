using System;
using System.Reflection;
using UnityEngine;

namespace LockStep.Game
{
    public class EventRegisterService : IEventRegisterService
    {
        public void RegisterEvent(object obj)
        {
            RegisterEvent<EventType,EventHandler>("OnEvent_", "OnEvent_".Length, Event);
        }

        public void UnRegisterEvent(object obj)
        {
            throw new NotImplementedException();
        }

        public void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen, Action<TEnum, TDelegate> callback, object obj) where TEnum : struct where TDelegate : Delegate
        {
            throw new NotImplementedException();
        }
    }
}