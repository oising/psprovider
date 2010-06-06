using System.Management.Automation;

namespace PSProviderFramework
{
    public sealed class PSModuleBoundDriveInfo : PSDriveInfo
    {
        public PSModuleBoundDriveInfo(PSDriveInfo drive, PSModuleInfo module)
            : base(drive)
        {
            this.Module = module;
        }

        public PSModuleInfo Module
        {
            get;
            private set;
        }
    }
}