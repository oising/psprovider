using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSProviderFramework
{
    [CmdletProvider("TreeScriptProvider", ProviderCapabilities.None)]
    public class TreeScriptProvider : NavigationCmdletProvider, IScriptProvider
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
                            new NotImplementedException("Drive-less operation not implemented."),
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

        //private void Dispatch(Action action, Expression<Action> expression)
        //{
        //    var methodCall = (MethodCallExpression)expression.Body;
        //    string methodName = methodCall.Method.Name;
        //    var parameterNames = (from memberExpr in methodCall.Arguments.Cast<MemberExpression>()
        //                          select ("$" + memberExpr.Member.Name)).ToArray();

        //    string invoke = String.Format("{0} {1};", methodName, String.Join(" ", parameterNames));
            
        //}

        //private TReturn Dispatch<TReturn>(Func<TReturn> func, Expression<Func<TReturn>> expression)
        //{
        //    return default(TReturn);
        //}

        //private TDelegate GetDispatcher<TDelegate>(Expression<TDelegate> expr)
        //{
        //    var methodCall = (MethodCallExpression) expr.Body;

        //    string methodName = methodCall.Method.Name;
        //    var parameterNames = (from memberExpr in methodCall.Arguments.Cast<MemberExpression>()
        //                          select ("$" + memberExpr.Member.Name)).ToArray();

        //    Console.WriteLine("Func: {0}({1});", methodName, String.Join(" ", parameterNames));

        //    TDelegate lambda;

        //    if (this.Module.ExportedFunctions.ContainsKey(methodName))
        //    {
        //        lambda = (TDelegate) 
        //    }
        //    else
        //    {
        //        lambda = expr.Compile();
        //    }
        //    return lambda;
        //}

        private TReturn InvokeFunction<TReturn>(string function, params object[] parameters)
        {
            TReturn returnValue;

            // push correct provider thread context for this call
            using (PSProviderContext<TreeScriptProvider>.Enter(this))
            {
                returnValue = PSProviderContext<TreeScriptProvider>.InvokeFunctionInternal<TReturn>(function, parameters);
            } // pop context

            return returnValue;
        }

        private void InvokeFunction(string function, params object[] parameters)
        {
            // push correct provider thread context for this call
            using (PSProviderContext<TreeScriptProvider>.Enter(this))
            {
                PSProviderContext<TreeScriptProvider>.InvokeFunctionInternal<object>(function, parameters);
            } // pop context
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
            module.SessionState.PSVariable.Set(new PSProviderVariable<TreeScriptProvider>());

            return new ScriptDriveInfo(drive, (PSModuleInfo) parameter.Value);
        }
    }
}