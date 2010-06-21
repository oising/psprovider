using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    public sealed class PSProviderVariable<TProvider> : PSVariable
        where TProvider : CmdletProvider, IScriptProvider
    {
        public PSProviderVariable()
            : base("PSProvider", null, ScopedItemOptions.Constant)
        {
            this.Description = string.Format("{0} Bound Provider", typeof(TProvider).Name);
        }

        public override bool IsValidValue(object value)
        {
            return (PSProviderContext<TProvider>.Current != null);
        }

        public override object Value
        {
            get
            {
                return PSProviderContext<TProvider>.Current;
            }
        }
    }
}