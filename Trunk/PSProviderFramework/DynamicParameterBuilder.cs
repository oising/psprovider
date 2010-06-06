using System;
using System.Management.Automation;

namespace PSProviderFramework
{
    public class DynamicParameterBuilder
    {
        private readonly RuntimeDefinedParameterDictionary _dictionary;

        public DynamicParameterBuilder()
        {
            _dictionary = new RuntimeDefinedParameterDictionary();
        }

        public void AddSwitchParam(string name)
        {
            AddParam<SwitchParameter>(name, false, null);
        }

        public void AddStringParam(string name)
        {
            AddStringParam(name, false);
        }

        public void AddStringParam(string name, bool mandatory)
        {
            AddStringParam(name, mandatory, null);
        }

        public void AddStringParam(string name, string parameterSet)
        {
            AddParam<String>(name, false, parameterSet);
        }

        public void AddStringParam(string name, bool mandatory, string parameterSet)
        {
            AddParam<String>(name, mandatory, parameterSet);
        }

        public void AddParam<T>(string name, bool mandatory, string parameterSet)
        {
            var pa = new ParameterAttribute {ParameterSetName = parameterSet, Mandatory = mandatory};
            var rdp = new RuntimeDefinedParameter {Name = name, ParameterType = typeof (T)};
            rdp.Attributes.Add(pa);

            _dictionary.Add(name, rdp);
        }

        public RuntimeDefinedParameterDictionary GetDictionary()
        {
            return _dictionary;
        }
    }
}
