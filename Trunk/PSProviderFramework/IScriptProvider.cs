using System.Management.Automation;

namespace PSProviderFramework
{
    public interface IScriptProvider
    {        
        PSModuleInfo Module { get; }
        IDynamicParameterBuilder GetParameterBuilder();
        void WriteItemObject(object item, string path, bool isContainer);
        void WriteWarning(string message);
        void WriteVerbose(string message);
        void WriteDebug(string message);
        void WriteError(ErrorRecord error);
        void WriteProgress(ProgressRecord progress);        
        void ThrowTerminatingError(ErrorRecord errorRecord);
    }
}