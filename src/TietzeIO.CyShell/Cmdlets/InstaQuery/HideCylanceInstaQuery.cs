using System;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.InstaQuery
{
    [Cmdlet(VerbsCommon.Hide, "CylanceInstaQuery")]
    [OutputType(typeof(CyInstaQuery))]

    public class HideCylanceInstaQuery : CyApiCmdlet
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
                    Connection.ArchiveInstaQueryAsync(Id).WaitAndUnwrapException();
                    break;

                case "ByInstaQuery":
                    Connection.ArchiveInstaQueryAsync(InstaQuery).WaitAndUnwrapException();
                    break;
            }
        }
    }
}
