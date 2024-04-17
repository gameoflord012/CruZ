using System;
using System.Collections.Generic;

namespace CruZ.GameEngine.GameSystem.UI
{
    public class UIRoot
    {
        internal UIRoot()
        {
            Control = new UIControl();
        }

        public UIControl AddBranch(string branchName)
        {
            if (_uiBranches.ContainsKey(branchName)) throw new InvalidOperationException();
            var branch = new UIControl();
            Control.AddChild(branch);
            return _uiBranches[branchName] = branch;
        }

        public UIControl GetBranch(string branchName)
        {
            return _uiBranches[branchName];
        }

        internal UIControl Control { get; private set; }

        Dictionary<string, UIControl> _uiBranches = [];
    }
}