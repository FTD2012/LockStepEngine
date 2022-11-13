using System;
using System.Collections.Concurrent;
using System.Threading;

namespace LockStepEngine
{
    public class OneThreadSynchronizationContext : SynchronizationContext
    {
        private readonly ConcurrentQueue<Action> queue = new ConcurrentQueue<Action>();

        public override void Post(SendOrPostCallback callback, object state)
        {
            Add(() => { callback(state); });
        }

        public void Update()
        {
            while (true)
            {
                queue.TryDequeue(out var action);
                if (action == null)
                {
                    return;
                }

                action();
            }
        }
        
        private void Add(Action action)
        {
            queue.Enqueue(action);
        }
    }
}