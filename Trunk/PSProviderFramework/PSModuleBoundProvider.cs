using System;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    [CmdletProvider("ModuleBoundProvider", ProviderCapabilities.None)]
    public class PSModuleBoundProvider : ContainerCmdletProvider
    {
        private PSModuleBoundDriveInfo ModuleBoundDriveInfo
        {
            get
            {
                return this.PSDriveInfo as PSModuleBoundDriveInfo;
            }
        }

        public new void WriteItemObject(object item, string path, bool isContainer)
        {
            base.WriteItemObject(item, path, isContainer);
        }

        public new void WriteWarning(string message)
        {
            base.WriteWarning(message);
        }

        public new void WriteVerbose(string message)
        {
            base.WriteVerbose(message);
        }

        public new void WriteDebug(string message)
        {
            base.WriteDebug(message);
        }

        public new void WriteError(ErrorRecord error)
        {
            base.WriteError(error);
        }

        public new void WriteProgress(ProgressRecord progress)
        {
            base.WriteProgress(progress);
        }

        private TReturn InvokeFunction<TReturn>(string function, params object[] parameters)
        {
            Debug.WriteLine(
                String.Format(
                    "InvokeFunction<{0}> {1} with {2} parameter(s).",
                    typeof (TReturn).Name,
                    function,
                    parameters.Length));

            using (PSProviderContext<PSModuleBoundProvider>.Enter(this))
            {
                try
                {
                    TReturn returnValue;

                    // try coerce function return to required type
                    object returned = ModuleBoundDriveInfo.Module.Invoke(
                        ScriptBlock.Create(function + " @args"),
                        parameters);

                    if (LanguagePrimitives.TryConvertTo(returned, out returnValue))
                    {
                        return returnValue;
                    }

                    throw new ArgumentException(
                        String.Format(
                            "Could not convert return value of function {0} to required type {1}. Returned type was {2}.",
                            function,
                            typeof(TReturn).Name,
                            (returned == null) ? "null" : returned.GetType().Name
                            ));
                }
                catch (MethodInvocationException ex)
                {
                    if (ex.InnerException is CommandNotFoundException)
                    {
                        this.WriteWarning(String.Format("Function {0} not found in bound module.", function));
                    }
                }
                catch (Exception ex)
                {
                    this.ThrowTerminatingError(
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
            }

            return default(TReturn);
        }

        private void InvokeFunction(string function, params object[] parameters)
        {
            InvokeFunction<object>(function, parameters);
        }

        protected override bool IsValidPath(string path)
        {
            return InvokeFunction<bool>("IsValidPath", path);
        }

        protected override void GetItem(string path)
        {
            InvokeFunction<object>("GetItem", path);
        }
        
        protected override void ClearItem(string path)
        {
            InvokeFunction("ClearItem", path);
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            InvokeFunction("GetChildItems", path, recurse);
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            InvokeFunction("GetChildNames", path, returnContainers);
        }

        protected override bool HasChildItems(string path)
        {
            return InvokeFunction<bool>("HasChildItems", path);
        }

        protected override object NewDriveDynamicParameters()
        {
            var builder = new DynamicParameterBuilder();
            builder.AddParam<PSModuleInfo>("ModuleInfo", true, null);
            return builder.GetDictionary();
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            var parameters = (RuntimeDefinedParameterDictionary) this.DynamicParameters;
            var parameter = parameters["ModuleInfo"];
            var module = ((PSModuleInfo) parameter.Value);            
            module.SessionState.PSVariable.Set(new PSProviderVariable());
            
            return new PSModuleBoundDriveInfo(drive, (PSModuleInfo)parameter.Value);
        }

        protected override void InvokeDefaultAction(string path)
        {
            InvokeFunction("InvokeDefaultAction", path);
        }

        protected override bool ItemExists(string path)
        {
            return InvokeFunction<bool>("ItemExists", path);
        }

    }
}
