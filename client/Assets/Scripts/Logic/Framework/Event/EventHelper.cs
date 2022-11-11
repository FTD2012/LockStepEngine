using System.Collections.Generic;

namespace LockStep
{
    public delegate void EventHandler(object param);

    public struct MessageInfo
    {
        public EventType eventType;
        public object param;

        public MessageInfo(EventType eventType, object param)
        {
            this.eventType = eventType;
            this.param = param;
        }
    }

    public struct ListenerInfo
    {
        public bool isRegister;
        public EventType eventType;
        public EventHandler eventHandler;

        public ListenerInfo(bool isRegister, EventType eventType, EventHandler eventHandler)
        {
            this.isRegister = isRegister;
            this.eventType = eventType;
            this.eventHandler = eventHandler;
        }
    }
    
    public class EventHelper
    {
        private static Dictionary<EventType, List<EventHandler>> eventListenerDic = new Dictionary<EventType, List<EventHandler>>();
        private static Queue<MessageInfo> pendingMessageInfoQueue = new Queue<MessageInfo>();
        private static Queue<ListenerInfo> pendingListenerInfoQueue = new Queue<ListenerInfo>();
        private static Queue<EventType> needRemoveEventQueue = new Queue<EventType>();
        private static bool isTriggingEvent;

        public static void AddListener(EventType eventType, EventHandler eventHandler)
        {
            if (isTriggingEvent)
            {
                pendingListenerInfoQueue.Enqueue(new ListenerInfo(true, eventType, eventHandler));
                return;
            }

            if (!eventListenerDic.TryGetValue(eventType, out var eventHandlerList))
            {
                eventHandlerList = new List<EventHandler>();
                eventListenerDic.Add(eventType, eventHandlerList);
            }
            eventHandlerList.Add(eventHandler);
        }

        public static void Trigger(EventType eventType, object param = null)
        {
            if (isTriggingEvent)
            {
                pendingMessageInfoQueue.Enqueue(new MessageInfo(eventType, param));
                return;
            }

            if (eventListenerDic.TryGetValue(eventType, out var eventHandlerList))
            {
                isTriggingEvent = true;
                foreach (var eventHandler in eventHandlerList)
                {
                    eventHandler.DynamicInvoke(param);
                    eventHandler.Invoke(param);
                }
            }

            isTriggingEvent = false;
            while (pendingListenerInfoQueue.Count > 0)
            {
                var listenerInfo = pendingListenerInfoQueue.Dequeue();
                if (listenerInfo.isRegister)
                {
                    AddListener(listenerInfo.eventType, listenerInfo.eventHandler);
                }
                else
                {
                    RemoveListener(listenerInfo.eventType, listenerInfo.eventHandler);
                }
            }

            while (needRemoveEventQueue.Count > 0)
            {
                RemoveListener(needRemoveEventQueue.Dequeue());
            }

            while (pendingMessageInfoQueue.Count > 0)
            {
                var messageInfo = pendingMessageInfoQueue.Dequeue();
                Trigger(messageInfo.eventType, messageInfo.param);
            }
        }

        public static void RemoveListener(EventType eventType, EventHandler eventHandler)
        {
            if (isTriggingEvent)
            {
                pendingListenerInfoQueue.Enqueue(new ListenerInfo(false, eventType, eventHandler));
                return;
            }

            if (!eventListenerDic.TryGetValue(eventType, out var eventHandlerList))
            {
                return;
            }

            if (!eventHandlerList.Remove(eventHandler))
            {
                return;
            }

            if (eventHandlerList.Count != 0)
            {
                return;
            }

            eventListenerDic.Remove(eventType);
        }

        public static void RemoveListener(EventType eventType)
        {
            if (isTriggingEvent)
            {
                needRemoveEventQueue.Enqueue(eventType);
                return;
            }

            eventListenerDic.Remove(eventType);
        }
    }
}