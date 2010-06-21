using System.Management.Automation;

namespace PSProviderFramework
{
    public interface IDynamicParameterBuilder
    {
        IDynamicParameterBuilder AddSwitchParam(string name);
        IDynamicParameterBuilder AddSwitchParam(string name, string parameterset);
        IDynamicParameterBuilder AddStringParam(string name);
        IDynamicParameterBuilder AddStringParam(string name, bool mandatory);
        IDynamicParameterBuilder AddStringParam(string name, string parameterSet);
        IDynamicParameterBuilder AddStringParam(string name, bool mandatory, string parameterSet);
        //IDynamicParameterBuilder Clear();
        RuntimeDefinedParameterDictionary GetDictionary();
    }
}