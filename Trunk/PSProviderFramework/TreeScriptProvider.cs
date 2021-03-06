﻿using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    [CmdletProvider("TransactedTreeScriptProvider", ProviderCapabilities.ShouldProcess | ProviderCapabilities.Transactions)]
    public class TransactedTreeScriptProvider : TreeScriptProvider {
        protected override TReturn InvokeFunction<TReturn>(string function, params object[] parameters) {
            TReturn returnValue;

            // push correct provider thread context for this call
            using (PSProviderContext<TransactedTreeScriptProvider>.Enter(this)) {
                returnValue = PSProviderContext<TransactedTreeScriptProvider>
                    .InvokeFunctionInternal<TReturn>(function, parameters);
            } // pop context

            return returnValue;
        }

        protected override void InvokeFunction(string function, params object[] parameters) {
            // push correct provider thread context for this call
            using (PSProviderContext<TransactedTreeScriptProvider>.Enter(this)) {
                PSProviderContext<TransactedTreeScriptProvider>
                    .InvokeFunctionInternal<object>(function, parameters);
            } // pop context
        }
    }

    [CmdletProvider("TreeScriptProvider", ProviderCapabilities.ShouldProcess)]
    public class TreeScriptProvider : NavigationCmdletProvider, IScriptProvider, IContentCmdletProvider, IPropertyCmdletProvider
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

        protected virtual TReturn InvokeFunction<TReturn>(string function, params object[] parameters)
        {
            TReturn returnValue;

            // push correct provider thread context for this call
            using (PSProviderContext<TreeScriptProvider>.Enter(this))
            {
                returnValue = PSProviderContext<TreeScriptProvider>.InvokeFunctionInternal<TReturn>(function, parameters);
            } // pop context

            return returnValue;
        }

        protected virtual void InvokeFunction(string function, params object[] parameters)
        {
            // push correct provider thread context for this call
            using (PSProviderContext<TreeScriptProvider>.Enter(this))
            {
                PSProviderContext<TreeScriptProvider>.InvokeFunctionInternal<object>(function, parameters);
            } // pop context
        }

        public new PSDriveInfo PSDriveInfo
        {
            get { return base.PSDriveInfo; }
        }

        protected override void GetItem(string path)
        {
            InvokeFunction("GetItem", path); // string path
        }

        protected override object GetItemDynamicParameters(string path)
        {
            return InvokeFunction<object>("GetItemDynamicParameters", path); // string path
        }

        protected override void SetItem(string path, object value)
        {
            InvokeFunction("SetItem", path, value); // string path, object value
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return InvokeFunction<object>("SetItemDynamicParameters", path, value); // string path, object value
        }

        protected override void ClearItem(string path)
        {
            InvokeFunction("ClearItem", path); // string path
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            return InvokeFunction<object>("ClearItemDynamicParameters", path); // string path
        }

        protected override void InvokeDefaultAction(string path)
        {
            InvokeFunction("InvokeDefaultAction", path); // string path
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return InvokeFunction<object>("InvokeDefaultActionDynamicParameters", path); // string path
        }

        protected override bool ItemExists(string path)
        {
            return InvokeFunction<bool>("ItemExists", path); // string path
        }

        protected override object ItemExistsDynamicParameters(string path)
        {
            return InvokeFunction<object>("ItemExistsDynamicParameters", path); // string path
        }

        protected override bool IsValidPath(string path)
        {
            return InvokeFunction<bool>("IsValidPath", path); // string path
        }

        protected override bool IsItemContainer(string path)
        {
            return InvokeFunction<bool>("IsItemContainer", path); // string path
        }

        protected override void MoveItem(string path, string destination)
        {
            InvokeFunction("MoveItem", path, destination); // string path, string destination
        }

        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return InvokeFunction<object>("MoveItemDynamicParameters", path, destination); // string path, string destination
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            InvokeFunction("GetChildItems", path, recurse); // string path, bool recurse
        }

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            return InvokeFunction<object>("GetChildItemsDynamicParameters", path, recurse); // string path, bool recurse
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            InvokeFunction("GetChildNames", path, returnContainers); // string path, ReturnContainers returnContainers
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return InvokeFunction<object>("GetChildNamesDynamicParameters", path); // string path
        }

        protected override void RenameItem(string path, string newName)
        {
            InvokeFunction("RenameItem", path, newName); // string path, string newName
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return InvokeFunction<object>("RenameItemDynamicParameters", path, newName); // string path, string newName
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            InvokeFunction("NewItem", path, itemTypeName, newItemValue); // string path, string itemTypeName, object newItemValue            
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return InvokeFunction<object>("NewItemDynamicParameters", path, itemTypeName, newItemValue); // string path, string itemTypeName, object newItemValue
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            InvokeFunction("RemoveItem", path, recurse); // string path, bool recurse
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return InvokeFunction<object>("RemoveItemDynamicParameters", path, recurse); // string path, bool recurse
        }

        protected override bool HasChildItems(string path)
        {
            return InvokeFunction<bool>("HasChildItems", path); // string path
        }

        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            InvokeFunction("CopyItem", path, copyPath, recurse); // string path, string copyPath, bool recurse
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            return InvokeFunction<object>("CopyItemDynamicParameters", path, destination, recurse); // string path, string destination, bool recurse
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

            var variableIntrinsics = module.SessionState.PSVariable;

            // fixme: should I check against Type too?
            if (variableIntrinsics.Get("PSProvider") == null)
            {
                variableIntrinsics.Set(new PSProviderVariable<TreeScriptProvider>());
            }            

            return new ScriptDriveInfo(drive, (PSModuleInfo) parameter.Value);
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