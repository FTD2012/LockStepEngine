namespace LockStepEngine
{
    public delegate void FuncUndoCommand(CommandNode minTickNode, CommandNode maxTickNode, object param);
    
    public interface ICommand
    {
        void Do(object param);
        void Undo(object param);
    }

    public interface ICommandBuffer
    {
        void Init(object param, FuncUndoCommand funcUndoCommand);
        void Jump(int curTick, int destTick);
        void Clean(int maxVerifiedTick);
        void Execute(int tick, ICommand command);
    }

    public class CommandNode
    {
        public CommandNode Pre;
        public CommandNode Next;
        public int Tick;
        public ICommand Command;

        public CommandNode(int tick, ICommand command, CommandNode pre = null, CommandNode next = null)
        {
            Tick = tick;
            Command = command;
            Pre = pre;
            Next = next;
        }
    }
    
    public class CommandBuffer : ICommandBuffer
    {
        private CommandNode head;
        private CommandNode tail;
        private object param;
        private FuncUndoCommand funcUndoCommand;

        public void Init(object _param, FuncUndoCommand _funcUndoCommand)
        {
            param = _param;
            funcUndoCommand = _funcUndoCommand;
        }

        public void Execute(int tick, ICommand command)
        {
            if (command == null)
            {
                return;
            }
            command.Do(param);
            
            var node = new CommandNode(tick, command, tail, null);
            if (head == null)
            {
                head = node;
                tail = node;
                return;
            }

            tail.Next = node;
            tail = node;
        }

        public void Jump(int curTick, int destTick)
        {
            if (tail == null || tail.Tick <= destTick)
            {
                return;
            }

            var newTail = tail;
            while (newTail.Pre != null && newTail.Pre.Tick >= destTick)
            {
                newTail = newTail.Pre;
            }

            var minTickNode = newTail;
            var maxTickNode = tail;
            if (newTail.Pre == null)
            {
                head = null;
                tail = null;
            }
            else
            {
                tail = newTail.Pre;
                tail.Next = null;
                newTail.Pre = null;
            }

            funcUndoCommand(minTickNode, maxTickNode, param);
        }

        private void UndoCommans(CommandNode minTickNode, CommandNode maxTickNode, object param)
        {
            if (maxTickNode == null)
            {
                return;
            }

            while (maxTickNode != minTickNode)
            {
                maxTickNode.Command.Undo(param);
                maxTickNode = maxTickNode.Pre;
            }
            maxTickNode.Command.Undo(param);
        }

        public void Clean(int maxVerfiedTick)
        {
            
        }
    }
}