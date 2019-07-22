using System;
using System.Management.Automation;
using Nito.AsyncEx.Synchronous;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.InstaQuery
{
    [Cmdlet(VerbsCommon.Get, "CylanceInstaQueries")]
    [OutputType(typeof(CyInstaQuery))]

    public class GetCylanceInstaQueries : CyApiCmdlet
    {
        [Parameter]
        public string Query { get; set; }

        [Parameter]
        public SwitchParameter IncludeArchived { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                RequestInstaQueriesAsync(Query, IncludeArchived.IsPresent).
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
