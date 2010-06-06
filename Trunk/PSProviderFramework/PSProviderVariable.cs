using System.Management.Automation;

namespace PSProviderFramework
{
    public sealed class PSProviderVariable : PSVariable
    {
        public PSProviderVariable()
            : base("PSProvider", null, ScopedItemOptions.Constant)
        {
            this.Description = "Bound Provider";
        }

        public override bool IsValidValue(object value)
        {
            return (PSProviderContext<PSModuleBoundProvider>.Current != null);
        }

        public override object Value
        {
            get
            {
                return PSProviderContext<PSModuleBoundProvider>.Current;
            }
        }
    }
}