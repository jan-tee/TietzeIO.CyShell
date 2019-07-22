using Nito.AsyncEx.Synchronous;
using System.Management.Automation;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Detection
{
    [Cmdlet(VerbsCommon.Get, "CylancePackages")]
    [OutputType(typeof(CyPackageMetaData))]

    public class GetCylancePackages : CyApiCmdlet
    {
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Connection.
                GetPackagesAsync().
                WaitAndUnwrapException().
                ForEach(WriteObject);
        }
    }
}
