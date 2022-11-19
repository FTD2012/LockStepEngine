using System.Collections.Generic;

namespace LockStepEngine
{
    public interface ITimeMachineContainer : ITimeMachineService
    {
        void RegisterTimeMachine(ITimeMachine roll);
    }

    public class TimeMachineContainer : ITimeMachineContainer
    {
        public int Tick { get; private set; }
        
        private List<ITimeMachine> timeMachineList;

        public void RegisterTimeMachine(ITimeMachine timeMachine)
        {
            timeMachineList.Add(timeMachine);
        }

        public void RollbackTo(int tick)
        {
            Tick = tick;
            for (int i = 0; i < timeMachineList.Count; i++)
            {
                timeMachineList[i].RollbackTo(tick);
            }
        }

        public void Backup(int tick)
        {
            Tick = tick;
            for (int i = 0; i < timeMachineList.Count; i++)
            {
                timeMachineList[i].Backup(tick);
            }
        }

        public void Clean(int maxVerifiedTick)
        {
            for (int i = 0; i < timeMachineList.Count; i++)
            {
                timeMachineList[i].Clean(maxVerifiedTick);
            }
        }
    }
}