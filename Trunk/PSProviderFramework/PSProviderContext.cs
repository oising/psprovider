//---------------------------------------------------------------------
// Author: jachymko
//
// Description: Holds current provider object instance.
//
// Creation Date: Feb 16, 2007
//---------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Provider;
using JetBrains.Annotations;

namespace PSProviderFramework
{
    public static class PSProviderContext<TProvider> where TProvider : CmdletProvider, IScriptProvider
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

        internal static TReturn InvokeFunctionInternal<TReturn>(string function, object[] parameters)
        {
            if (function == null)
            {
                throw new ArgumentNullException("function");
            }

            TReturn returnValue = default(TReturn);

            // dynamic parameters not supported yet
            if (function.IndexOf("dynamic", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return returnValue;
            }

            Debug.WriteLine(
                String.Format(
                    "InvokeFunction<{0}> {1} with {2} parameter(s).",
                    typeof (TReturn).Name,
                    function,
                    parameters.Length));
           
            try
            {
                // function exists in bound module?
                if (Current.Module.ExportedFunctions.ContainsKey(function))
                {
                    // call module function, splatting parameter(s), if any.
                    object returned = Current.Module.Invoke(
                        ScriptBlock.Create(function + " @args"),
                        parameters);

                    // try coerce function return to required type
                    if (LanguagePrimitives.TryConvertTo(returned, out returnValue))
                    {
                        Current.WriteDebug("ScriptProvider: conversion success.");
                    }
                    else
                    {
                        throw new ArgumentException(
                            String.Format(
                                "Could not convert return value of function {0} to required type {1}. Returned type was {2}.",
                                function,
                                typeof (TReturn).Name,
                                (returned == null) ? "null" : returned.GetType().Name
                                ));
                    }
                }
            }
            catch (Exception ex)
            {
                Current.ThrowTerminatingError(
                    new ErrorRecord(
                        new ProviderInvocationException(
                            String.Format(
                                "Module invocation error calling {0}: {1}",
                                function,
                                ex.Message)),
                        "BoundModuleMethodInvocationError",
                        ErrorCategory.InvalidResult,
                        null));
            }
            return returnValue;

        }
    }
}
