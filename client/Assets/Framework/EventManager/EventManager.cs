using System;
using System.Collections;
using System.Collections.Generic;

namespace Framework
{
    public class EventManager
    {
        private static readonly Dictionary<string, Delegate> listenerDic = new Dictionary<string, Delegate>();

        public static void AddEventListener(string eventID, Action handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action)listenerDic[eventID] + handler;
            }
        }

        public static void AddEventListener<T>(string eventID, Action<T> handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action<T>)listenerDic[eventID] + handler;
            }
        }

        public static void AddEventListener<T, U>(string eventID, Action<T, U> handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U>)listenerDic[eventID] + handler;
            }
        }

        public static void AddEventListener<T, U, V>(string eventID, Action<T, U, V> handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V>)listenerDic[eventID] + handler;
            }
        }

        public static void AddEventListener<T, U, V, W>(string eventID, Action<T, U, V, W> handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V, W>)listenerDic[eventID] + handler;
            }
        }

        public static void AddEventListener<T, U, V, W, X>(string eventID, Action<T, U, V, W, X> handler)
        {
            if (OnListenerAdding(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V, W, X>)listenerDic[eventID] + handler;
            }
        }

        public static void TriggerEvent(string eventID)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    Delegate[] callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action;
                        if (callback == null)
                        {
                            return;
                        }

                        callback();
                    }
                }
            }
        }

        public static void TriggerEvent<T>(string eventID, T arg1)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    var callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action<T>;
                        if (callback == null)
                        {
                            return;
                        }

                        callback(arg1);
                    }
                }
            }
        }

        public static void TriggerEvent<T, U>(string eventID, T arg1, U arg2)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    var callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action<T, U>;
                        if (callback == null)
                        {
                            return;
                        }

                        callback(arg1, arg2);
                    }
                }
            }
        }

        public static void TriggerEvent<T, U, V>(string eventID, T arg1, U arg2, V arg3)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    var callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action<T, U, V>;
                        if (callback == null)
                        {
                            return;
                        }

                        callback(arg1, arg2, arg3);
                    }
                }
            }
        }

        public static void TriggerEvent<T, U, V, W>(string eventID, T arg1, U arg2, V arg3, W arg4)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    var callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action<T, U, V, W>;
                        if (callback == null)
                        {
                            return;
                        }

                        callback(arg1, arg2, arg3, arg4);
                    }
                }
            }
        }

        public static void TriggerEvent<T, U, V, W, X>(string eventID, T arg1, U arg2, V arg3, W arg4, X arg5)
        {
            Delegate d;
            if (listenerDic.TryGetValue(eventID, out d))
            {
                if (d != null)
                {
                    var callbacks = d.GetInvocationList();
                    for (int i = 0; i < callbacks.Length; i++)
                    {
                        var callback = callbacks[i] as Action<T, U, V, W, X>;
                        if (callback == null)
                        {
                            return;
                        }

                        callback(arg1, arg2, arg3, arg4, arg5);
                    }
                }
            }
        }


        public static void RemoveEventListener(string eventID, Action handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action)listenerDic[eventID] - handler;
            }
        }

        public static void RemoveEventListener<T>(string eventID, Action<T> handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action<T>)listenerDic[eventID] - handler;
            }
        }

        public static void RemoveEventListener<T, U>(string eventID, Action<T, U> handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U>)listenerDic[eventID] - handler;
            }
        }

        public static void RemoveEventListener<T, U, V>(string eventID, Action<T, U, V> handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V>)listenerDic[eventID] - handler;
            }
        }

        public static void RemoveEventListener<T, U, V, W>(string eventID, Action<T, U, V, W> handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V, W>)listenerDic[eventID] - handler;
            }
        }

        public static void RemoveEventListener<T, U, V, W, X>(string eventID, Action<T, U, V, W, X> handler)
        {
            if (OnListenerRemoving(eventID, handler))
            {
                listenerDic[eventID] = (Action<T, U, V, W, X>)listenerDic[eventID] - handler;
            }
        }
        
        public static bool HasEventListener(string eventID)
        {
            return listenerDic.ContainsKey(eventID);
        }

        public static bool HasEventListener(string eventID, Action handler)
        {
            if (!HasEventListener(eventID))
            {
                return false;
            }

            var InvocationList = listenerDic[eventID].GetInvocationList();
            for (int i = 0; i < InvocationList.Length; i++)
            {
                if (InvocationList[i] == (Delegate)handler)
                {
                    return true;
                }
            }

            return false;
        }
        
        private static bool OnListenerAdding(string eventID, Delegate listener)
        {
            if (!listenerDic.ContainsKey(eventID))
            {
                listenerDic.Add(eventID, null);
            }

            Delegate d = listenerDic[eventID];
            if (d != null && d.GetType() != listener.GetType())
            {
                GLog.Error($"Try to add not correct event {eventID}. Current type is {d.GetType().Name}, adding type is {listener.GetType().Name}.");
                return false;
            }

            if (d != null && ((IList)d.GetInvocationList()).Contains(listener))
            {
                GLog.Error($"Try to add identical delegate to {eventID}. Current type is {d.GetType().Name}, adding type is {listener.GetType().Name}.");
                return false;
            }

            return true;
        }

        private static bool OnListenerRemoving(string eventID, Delegate listener)
        {
            if (!listenerDic.ContainsKey(eventID))
            {
                return false;
            }

            Delegate d = listenerDic[eventID];
            if ((d != null) && (d.GetType() != listener.GetType()))
            {
                GLog.Error($"Remove listener {eventID}\" failed, Current type is {d.GetType()}, adding type is {listener.GetType()}.");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void ClearListener()
        {
            listenerDic.Clear();
        }
    }
}