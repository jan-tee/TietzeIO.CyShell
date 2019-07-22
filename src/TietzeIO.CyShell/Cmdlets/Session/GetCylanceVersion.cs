using System;
using System.Management.Automation;
using System.Reflection;
using TietzeIO.CyAPI;
using TietzeIO.CyShell.Cmdlets.Base;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Session
{
    [Cmdlet(VerbsCommon.Get, "CylanceVersion")]
    [OutputType(typeof(ModuleInfo))]
    public class GetCylanceVersion : CyCmdlet
    {
        public class ModuleInfo
        {
            public AssemblyName CyShellVersion = Assembly.GetExecutingAssembly().GetName();
            public AssemblyName CyAPIVersion = Assembly.GetAssembly(typeof(ApiV2)).GetName();
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            WriteObject(new ModuleInfo());
        }
    }
}
