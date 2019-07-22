using System;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Console
{
    [Cmdlet(VerbsCommon.Get, "CylanceConsole")]
    public class GetCylanceConsole : CyCmdlet, IDynamicParameters
    {
        private PowershellDynamicParameter ConsolesParam;
        public object GetDynamicParameters()
        {
            // return new ConsoleDynamicParameter();
            ConsolesParam = new PowershellDynamicParameter(this, "Console", (from c in ConfigurationManager.Default.All() orderby c.ConsoleId select c.ConsoleId).ToArray());
            ConsolesParam.Mandatory = false;
            ConsolesParam.Position = 1;
            return ConsolesParam.GetParamDictionary();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            var consoles = ConfigurationManager.Default.All();
            WriteObject(consoles.Where(c =>
            {
                if (ConsolesParam.Value != null)
                {
                    return ConsolesParam.Value.Equals(c.ConsoleId, StringComparison.InvariantCultureIgnoreCase);
                }
                else
                {
                    return true;
                }
            }), true);
        }
    }
}