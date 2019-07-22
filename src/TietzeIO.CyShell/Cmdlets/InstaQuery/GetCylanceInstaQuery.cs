using System;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.InstaQuery
{
    [Cmdlet(VerbsCommon.Get, "CylanceInstaQuery")]
    [OutputType(typeof(CyInstaQuery))]

    public class GetCylanceInstaQuery : CyApiCmdlet
    {
        [Parameter(Mandatory = false, ParameterSetName = "ById")]
        public string Id { get; set; }

        [Parameter(Mandatory = false, ParameterSetName = "ByInstaQuery", ValueFromPipeline = true)]
        public CyInstaQuery InstaQuery { get; set; }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ById":
                    var r1 = Connection.RequestInstaQueryAsync(Id).WaitAndUnwrapException();
                    if (r1 != null)
                    {
                        WriteObject(r1);
                    }
                    break;

                case "ByInstaQuery":
                    var r2 = Connection.RequestInstaQuery(InstaQuery).WaitAndUnwrapException();
                    if (r2 != null)
                    {
                        WriteObject(r2);
                    }
                    break;
            }
        }
    }
}
