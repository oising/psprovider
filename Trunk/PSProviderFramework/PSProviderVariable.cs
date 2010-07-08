using System;
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

        public override object Value
        {
            get
            {
                TProvider provider = PSProviderContext<TProvider>.Current;
                if (provider == null)
                {
                    throw new InvalidOperationException("You cannot access the $PSProvider variable from here.");
                }
                return provider;
            }
        }
    }
}