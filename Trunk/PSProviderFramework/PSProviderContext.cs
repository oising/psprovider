//---------------------------------------------------------------------
// Author: jachymko
//
// Description: Holds current provider object instance.
//
// Creation Date: Feb 16, 2007
//---------------------------------------------------------------------

using System;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    public static class PSProviderContext<TProvider> where TProvider : CmdletProvider
    {
        public static TProvider Current
        {
            get { return (TProvider)PSProviderThreadContext.Current(typeof(TProvider)); }
        }

        public static PSProviderThreadContext.Cookie Enter(TProvider provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            return PSProviderThreadContext.Enter(typeof(TProvider), provider);
        }
    }
}
