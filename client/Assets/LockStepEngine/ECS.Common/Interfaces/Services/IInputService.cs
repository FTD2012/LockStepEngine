using System.Collections.Generic;

namespace LockStepEngine
{
    public interface IInputService : IService
    {
        void Execute(InputCmd inputCmd, object entity);
        List<InputCmd> GetInputCmdList();
        List<InputCmd> GetDebugInputCmdList();
    }
}