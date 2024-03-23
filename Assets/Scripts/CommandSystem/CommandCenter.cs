using System.Collections.Generic;
using UnityEngine;

namespace GG.CommandSystem
{
    public static class CommandCenter
    {
        public static bool IsUndoAvailable
        {
            get
            {
                return _commandStack.Count > 0;
            }
        }

        private static Stack<Command> _commandStack;

        static CommandCenter()
        {
            _commandStack = new();
        }

        public static void ExecuteCommand(Command command)
        {
            if (!command.IsExecutable())
            {
                Debug.LogWarning("Can not execute command because it is not executable");
                return;
            }

            _commandStack.Push(command);
            command.Execute();
        }

        public static void Undo()
        {
            if (_commandStack.Count == 0) return;

            var command = _commandStack.Pop();
            command.Undo();
            command.Dispose();
        }

        public static void ClearStack()
        {
            _commandStack.Clear();
        }
    }
}