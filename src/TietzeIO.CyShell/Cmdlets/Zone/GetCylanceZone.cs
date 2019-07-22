using System;
using System.Linq;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Zone
{
    [Cmdlet(VerbsCommon.Get, "CylanceZone")]
    [OutputType(typeof(CyZone))]
    public class GetCylanceZone : CyApiCmdlet
    {
        [Parameter(ValueFromPipeline = true, ParameterSetName = "ByZone")]
        public CyZone Zone { get; set; }

        [Parameter(ParameterSetName = "ByName")]
        public string Name { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            switch (ParameterSetName)
            {
                case "ByName":
                    var zones = Connection.
                        RequestZoneListAsync().
                        WaitAndUnwrapException();
                    if (zones == null) return;
                    var zone = from z in zones where string.Equals(z.name, Name, StringComparison.InvariantCultureIgnoreCase) select z;
                    if (zone.Any())
                    {
                        WriteObject(zone.First());
                    }
                    break;
            }
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if (null != Zone)
            {
                var zone = Connection.RequestZone(Zone);
                if (zone != null)
                {
                    WriteObject(zone);
                }
            }
        }
    }
}
