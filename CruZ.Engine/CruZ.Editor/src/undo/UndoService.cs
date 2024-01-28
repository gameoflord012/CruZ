using System;
using System.Collections.Generic;

namespace CruZ.Editor.Systems
{
    public interface ICanUndo
    { 
        object CaptureState();
        void RestoreState(object state);
    }

    public static class UndoService
    {
        public static List<ICanUndo> Registers = [];
        public static List<Dictionary<ICanUndo, object>> stateStack = [];
        private static int _stackInd = -1;

        public static void Capture()
        {
            return;
            if(_stackInd < stateStack.Count - 1)
                stateStack.RemoveRange(_stackInd + 1, stateStack.Count - _stackInd - 1);

            var state = new Dictionary<ICanUndo, object>();
            
            foreach (var register in Registers)
            {
                state[register] = register.CaptureState();
            }

            stateStack.Add(state);
            _stackInd++;
        }

        public static void Undo()
        {
            if(_stackInd >= 0)
            {
                Restore();
                _stackInd--;
            }
        }

        public static void Redo()
        {
            if(_stackInd < stateStack.Count - 1)
            {
                _stackInd++;
                Restore();
            }
        }

        private static void Restore()
        {
            foreach (var register in Registers)
            {
                register.RestoreState(stateStack[_stackInd][register]);
            }
        }
    }
}
