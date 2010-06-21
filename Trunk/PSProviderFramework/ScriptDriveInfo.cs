using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    public sealed class ScriptDriveInfo : PSDriveInfo
    {
        public ScriptDriveInfo(PSDriveInfo drive, PSModuleInfo module)
            : base(drive)
        {
            Debug.Assert(module != null, "module != null");
            this.Module = module;
        }

        public PSModuleInfo Module
        {
            get;
            private set;
        }
    }
}