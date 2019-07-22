using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Configuration.Data;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Policy;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Util;

namespace TietzeIO.CyShell.Cmdlets.Policy
{
    [Cmdlet(VerbsLifecycle.Enable, "CylanceApplicationTemplate")]
    [OutputType(typeof(CyPolicy))]
    public class EnableCylanceApplicationTemplate : CyCmdlet, IDynamicParameters
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true)]
        public CyPolicy Policy { get; set; }

        public PowershellDynamicParameter TemplateParam;

        List<ExclusionTemplate> _templates = new List<ExclusionTemplate>(CyAPI.Configuration.ConfigurationData.Default.ApplicationTemplates.Values);

        public object GetDynamicParameters()
        {
            TemplateParam = new PowershellDynamicParameter(this, "Template",
                (from template in _templates orderby template.name select template.name).ToArray());
            TemplateParam.AllowMultiple = true;
            TemplateParam.Mandatory = true;
            TemplateParam.Position = 2;
            return TemplateParam.GetParamDictionary();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();

            // don't update the original object, but create a new copy - uphold powershell conventions
            var policy = Policy.GetClone(false);

            foreach (var template_name in TemplateParam.Values)
            {
                var template = (from t in _templates where template_name.Equals(t.name, StringComparison.InvariantCultureIgnoreCase) select t).Single();
                template.Apply(policy);
            }
            WriteObject(policy);
        }
    }
}
