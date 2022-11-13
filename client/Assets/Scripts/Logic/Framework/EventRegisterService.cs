using System;
using System.Reflection;
using UnityEngine;

namespace LockStepEngine.Game
{
    public class EventRegisterService : IEventRegisterService
    {
        public void RegisterEvent(object obj)
        {
            RegisterEvent<EventType,EventHandler>("OnEvent_", "OnEvent_".Length, EventHelper.AddListener, obj);
        }

        public void UnRegisterEvent(object obj)
        {
            RegisterEvent<EventType, EventHandler>("OnEvent_", "OnEvent_".Length, EventHelper.RemoveListener, obj);
        }

        public void RegisterEvent<TEnum, TDelegate>(string prefix, int ignorePrefixLen, Action<TEnum, TDelegate> callback, object obj) where TEnum : struct where TDelegate : Delegate
        {
            if (callback == null)
            {
                return;
            }
            
            var methodInfos = obj.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var methodInfo in methodInfos)
            {
                var methodName = methodInfo.Name;
                if (methodName.StartsWith(prefix))
                {
                    var eventTypeStr = methodName.Substring(ignorePrefixLen);
                    if (Enum.TryParse(eventTypeStr, out TEnum eType))
                    {
                        try
                        {
                            var handler = CreateDelegateFromMethodInfo<TDelegate>(obj, methodInfo);
                            callback(eType, handler);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("method name " + methodName);
                        }
                    }
                }
            }
        }

        private static T CreateDelegateFromMethodInfo<T>(object instance, MethodInfo method) where T : Delegate
        {
            return Delegate.CreateDelegate(typeof(T), instance, method) as T;
        }
    }
}