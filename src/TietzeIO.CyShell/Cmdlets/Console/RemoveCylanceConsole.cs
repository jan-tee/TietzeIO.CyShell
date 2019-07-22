using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Console
{
    [Cmdlet(VerbsCommon.Remove, "CylanceConsole")]
    public class RemoveCylanceConsole : CyCmdlet, IDynamicParameters
    {
        private PowershellDynamicParameter ConsolesParam;
        public object GetDynamicParameters()
        {
            // return new ConsoleDynamicParameter();
            ConsolesParam = new PowershellDynamicParameter(this, "Console", (from c in ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            ConsolesParam.Mandatory = true;
            ConsolesParam.Position = 1;
            return ConsolesParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            ConfigurationManager.Default.Remove(ConsolesParam.Value);
        }
    }
}
