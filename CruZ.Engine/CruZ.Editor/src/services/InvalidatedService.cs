using System.Collections.Generic;
using System.Windows.Forms;

namespace CruZ.Editor.Services
{
    class InvalidatedService
    {
        public static void Register(Control controls, params string[] invalidatedStrings)
        {
            foreach (var invalidatedString in invalidatedStrings)
            {
                GetRegisters(invalidatedString).Remove(controls);
                GetRegisters(invalidatedString).Add(controls);
            }
        }

        public static void SendInvalidated(string invalidatedString)
        {
            foreach (var control in GetRegisters(invalidatedString))
            {
                control.Refresh();
            }
        }

        static List<Control> GetRegisters(string invalidatedString)
        {
            if (!registers.ContainsKey(invalidatedString))
                registers[invalidatedString] = [];

            return registers[invalidatedString];
        }

        static Dictionary<string, List<Control>> registers = [];
    }
}