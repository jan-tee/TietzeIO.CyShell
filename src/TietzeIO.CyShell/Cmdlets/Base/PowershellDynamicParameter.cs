using System;
using System.Collections.ObjectModel;
using System.Management.Automation;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    /// <summary>
    /// Creates a Powershell dynamic parameter (a parameter with a ValidateSet)
    /// </summary>
    public class PowershellDynamicParameter
    {
        private string[] ValidateSetValues;
        private string Name { get; set; }
        public bool AllowMultiple { get; set; }
        public string HelpMessage { get; set; }
        public string ParameterSetName { get; set; }
        public bool Mandatory { get; set; }
        public int? Position { get; set; }

        public string Value
        {
            get
            {
                if (!Cmdlet.MyInvocation.BoundParameters.ContainsKey(Name))
                {
                    return null;
                }
                return Cmdlet.MyInvocation.BoundParameters[Name] as string;
            }
        }
        public string[] Values
        {
            get
            {
                if (!Cmdlet.MyInvocation.BoundParameters.ContainsKey(Name))
                {
                    return null;
                }
                return Cmdlet.MyInvocation.BoundParameters[Name] as string[];
            }
        }

        public PSCmdlet Cmdlet { get; private set; }

        public PowershellDynamicParameter(PSCmdlet parent, string name, params string[] values)
        {
            ValidateSetValues = values;
            AllowMultiple = false;
            Name = name;
            Position = null;
            Mandatory = false;
            Cmdlet = parent;
        }

        public RuntimeDefinedParameterDictionary GetParamDictionary()
        {
            var paramDictionary = new RuntimeDefinedParameterDictionary();
            AddToParamDictionary(paramDictionary);
            return paramDictionary;
        }

        public void AddToParamDictionary(RuntimeDefinedParameterDictionary paramDictionary)
        {
            var attributes = new Collection<Attribute>();

            var parameterAttribute = new ParameterAttribute();
            parameterAttribute.Mandatory = true;
            if (null != Position) parameterAttribute.Position = 1;
            if (null != HelpMessage) parameterAttribute.HelpMessage = HelpMessage;
            if (null != ParameterSetName) parameterAttribute.ParameterSetName = ParameterSetName;
            parameterAttribute.Mandatory = Mandatory;
            attributes.Add(parameterAttribute);

            if (null != ValidateSetValues)
            {
                var validateSetAttribute = new ValidateSetAttribute(ValidateSetValues);
                attributes.Add(validateSetAttribute);
            }

            RuntimeDefinedParameter OptionsRuntimeDefinedParam;
            if (AllowMultiple)
                OptionsRuntimeDefinedParam = new RuntimeDefinedParameter(Name, typeof(string[]), attributes);
            else
                OptionsRuntimeDefinedParam = new RuntimeDefinedParameter(Name, typeof(string), attributes);

            paramDictionary.Add(Name, OptionsRuntimeDefinedParam);
        }
    }
}
