using System;
using System.Management.Automation;

namespace PSProviderFramework
{
    public class DynamicParameterBuilder : IDynamicParameterBuilder
    {
        private readonly RuntimeDefinedParameterDictionary _dictionary;

        internal DynamicParameterBuilder()
        {
            _dictionary = new RuntimeDefinedParameterDictionary();
        }


        public IDynamicParameterBuilder AddSwitchParam(string name)
        {
            return AddParam<SwitchParameter>(name, false, null);
        }

        public IDynamicParameterBuilder AddSwitchParam(string name, string parameterset)
        {
            return AddParam<SwitchParameter>(name, false, parameterset);
        }

        public IDynamicParameterBuilder AddStringParam(string name)
        {
            return AddStringParam(name, false);
        }

        public IDynamicParameterBuilder AddStringParam(string name, bool mandatory)
        {
            return AddStringParam(name, mandatory, null);
        }

        public IDynamicParameterBuilder AddStringParam(string name, string parameterSet)
        {
            return AddParam<String>(name, false, parameterSet);
        }

        public IDynamicParameterBuilder AddStringParam(string name, bool mandatory, string parameterSet)
        {
            return AddParam<String>(name, mandatory, parameterSet);            
        }

        internal IDynamicParameterBuilder AddParam<T>(string name, bool mandatory, string parameterSet)
        {
            var pa = new ParameterAttribute {ParameterSetName = parameterSet, Mandatory = mandatory};
            var rdp = new RuntimeDefinedParameter {Name = name, ParameterType = typeof (T)};
            rdp.Attributes.Add(pa);

            _dictionary.Add(name, rdp);
            
            return this;
        }

        //public IDynamicParameterBuilder Clear()
        //{
        //    _dictionary.Clear();
        //    return this;
        //}

        public RuntimeDefinedParameterDictionary GetDictionary()
        {
            return _dictionary;
        }
    }
}
