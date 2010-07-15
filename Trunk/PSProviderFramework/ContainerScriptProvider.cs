using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    [CmdletProvider("ContainerScriptProvider", ProviderCapabilities.ShouldProcess)]
    public class ContainerScriptProvider : ContainerCmdletProvider, IScriptProvider, IContentCmdletProvider, IPropertyCmdletProvider
    {
        private ScriptDriveInfo CurrentDrive
        {
            get { return PSDriveInfo as ScriptDriveInfo; }
        }

        #region IScriptProvider Members

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

        public IDynamicParameterBuilder GetParameterBuilder()
        {
            return new DynamicParameterBuilder();
        }

        public PSModuleInfo Module
        {
            get
            {
                if (CurrentDrive == null)
                {
                    ThrowTerminatingError(
                        new ErrorRecord(
                            new NotImplementedException("Driveless operation not implemented."),
                            "NoCurrentScriptDrive",
                            ErrorCategory.NotImplemented,
                            null));
                }
                // ReSharper disable PossibleNullReferenceException
                return CurrentDrive.Module;
                // ReSharper restore PossibleNullReferenceException
            }
        }

        #endregion

        protected override object GetItemDynamicParameters(string path)
        {
            return InvokeFunction<object>("GetItemDynamicParameters"); // string path
        }

        protected override void SetItem(string path, object value)
        {
            InvokeFunction("SetItem", path, value); // string path, object value
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return InvokeFunction<object>("SetItemDynamicParameters", path, value); // string path, object value
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            return InvokeFunction<object>("ClearItemDynamicParameters"); // string path
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return InvokeFunction<object>("InvokeDefaultActionDynamicParameters"); // string path
        }

        protected override object ItemExistsDynamicParameters(string path)
        {
            return InvokeFunction<object>("ItemExistsDynamicParameters"); // string path
        }

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            return InvokeFunction<object>("GetChildItemsDynamicParameters"); // string path, bool recurse
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return InvokeFunction<object>("GetChildNamesDynamicParameters"); // string path
        }

        protected override void RenameItem(string path, string newName)
        {
            InvokeFunction("RenameItem", path, newName); // string path, string newName
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return InvokeFunction<object>("RenameItemDynamicParameters"); // string path, string newName
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            InvokeFunction("NewItem", path, itemTypeName, newItemValue);
            // string path, string itemTypeName, object newItemValue
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return InvokeFunction<object>("NewItemDynamicParameters");
            // string path, string itemTypeName, object newItemValue
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            InvokeFunction("RemoveItem", path, recurse); // string path, bool recurse
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return InvokeFunction<object>("RemoveItemDynamicParameters", path, recurse); // string path, bool recurse
        }

        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            InvokeFunction("CopyItem", path, copyPath, recurse); // string path, string copyPath, bool recurse
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            return InvokeFunction<object>("CopyItemDynamicParameters", path, destination, recurse); // string path, string destination, bool recurse
        }

        private TReturn InvokeFunction<TReturn>(string function, params object[] parameters)
        {
            TReturn returnValue;

            // push correct provider thread context for this call
            using (PSProviderContext<ContainerScriptProvider>.Enter(this))
            {
                returnValue = PSProviderContext<ContainerScriptProvider>.InvokeFunctionInternal<TReturn>(function,
                                                                                                         parameters);
            } // pop context

            return returnValue;
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
            var parameters = (RuntimeDefinedParameterDictionary) DynamicParameters;
            var parameter = parameters["ModuleInfo"];
            var module = ((PSModuleInfo) parameter.Value);
            module.SessionState.PSVariable.Set(new PSProviderVariable<ContainerScriptProvider>());

            return new ScriptDriveInfo(drive, (PSModuleInfo) parameter.Value);
        }

        protected override void InvokeDefaultAction(string path)
        {
            InvokeFunction("InvokeDefaultAction", path);
        }

        protected override bool ItemExists(string path)
        {
            return InvokeFunction<bool>("ItemExists", path);
        }

        #region Implementation of IContentCmdletProvider

        IContentReader IContentCmdletProvider.GetContentReader(string path)
        {
            return InvokeFunction<IContentReader>("GetContentReader", path);
        }

        object IContentCmdletProvider.GetContentReaderDynamicParameters(string path)
        {
            return null;
        }

        IContentWriter IContentCmdletProvider.GetContentWriter(string path)
        {
            return InvokeFunction<IContentWriter>("GetContentWriter", path);
        }

        object IContentCmdletProvider.GetContentWriterDynamicParameters(string path)
        {
            return null;
        }

        void IContentCmdletProvider.ClearContent(string path)
        {
            InvokeFunction("ClearContent", path);
        }

        object IContentCmdletProvider.ClearContentDynamicParameters(string path)
        {
            return null;
        }

        #endregion

        #region Implementation of IPropertyCmdletProvider

        void IPropertyCmdletProvider.GetProperty(string path, Collection<string> providerSpecificPickList)
        {
            InvokeFunction("GetProperty", path, providerSpecificPickList);
        }

        object IPropertyCmdletProvider.GetPropertyDynamicParameters(string path, Collection<string> providerSpecificPickList)
        {
            return null;
        }

        void IPropertyCmdletProvider.SetProperty(string path, PSObject propertyValue)
        {
            InvokeFunction("SetProperty", path, propertyValue);
        }

        object IPropertyCmdletProvider.SetPropertyDynamicParameters(string path, PSObject propertyValue)
        {
            return null;
        }

        void IPropertyCmdletProvider.ClearProperty(string path, Collection<string> propertyToClear)
        {
            InvokeFunction("ClearProperty", path, propertyToClear);
        }

        object IPropertyCmdletProvider.ClearPropertyDynamicParameters(string path, Collection<string> propertyToClear)
        {
            return null;
        }

        #endregion
    }
}