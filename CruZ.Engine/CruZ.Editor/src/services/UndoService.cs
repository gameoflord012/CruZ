using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CruZ.Editor.Services
{
    public interface ICanUndo
    { 
        object CaptureState();
        void RestoreState(object state);
        bool StatesIdentical(object stateA, object stateB);
    }

    public static class UndoService
    {
        public static List<KeyValuePair<ICanUndo, object>> stateStack = [];
        private static int _stackInd = -1;

        public static void Capture(ICanUndo register)
        {
            if (_stackInd < stateStack.Count - 1)
                stateStack.RemoveRange(_stackInd + 1, stateStack.Count - _stackInd - 1);

            var capture = register.CaptureState();

            if (!Identical(register, capture))
            {
                Push(register, capture);
            }
        }

        public static void Undo()
        {
            while(_stackInd > 0 && Unchanged())
            {
                Pop();
            }

            Restore();
        }

        public static void Redo()
        {
            if(_stackInd < stateStack.Count - 1)
            {
                _stackInd++;
                Restore();
            }
        }

        public static bool Unchanged()
        {
            return Identical(
                PeakRegister(),
                PeakRegister().CaptureState());
        }

        private static void Restore()
        {
            if(StackEmpty()) return;
            PeakRegister().RestoreState(PeakStateObj());
        }

        private static bool Identical(ICanUndo register, object state)
        {
            if(_stackInd < 0)               return false;
            if(register != PeakRegister())  return false;

            return register.StatesIdentical(PeakStateObj(), state);
        }

        private static void Push(ICanUndo register, object state)
        {
            stateStack.Add(new(register, state));
            _stackInd++;
        }

        private static void Pop()
        {
            if(_stackInd < 0) return;
            _stackInd--;
        }

        private static KeyValuePair<ICanUndo, object> Peak()
        {
            Trace.Assert(_stackInd >= 0);
            return stateStack[_stackInd];
        }

        private static ICanUndo PeakRegister()
        {
            return Peak().Key;
        }

        private static object PeakStateObj()
        {
            return Peak().Value;
        }

        private static bool StackEmpty()
        {
            return _stackInd < 0;
        }
    }
}
