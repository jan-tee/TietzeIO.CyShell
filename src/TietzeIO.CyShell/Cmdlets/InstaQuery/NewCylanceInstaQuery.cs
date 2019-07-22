using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.InstaQuery
{
    [Cmdlet(VerbsCommon.New, "CylanceInstaQuery")]
    [OutputType(typeof(CyInstaQuery))]

    public class NewCylanceInstaQuerý : CyApiCmdlet
    {
        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter]
        public string Description { get; set; } = "CyShell initiated InstaQuery";

        [Parameter(Mandatory = true)]
        [ValidateSet("File.Path",
            "File.MD5",
            "File.SHA256",
            "File.Owner",
            "File.CreationDateTime",
            "Process.Name",
            "Process.CommandLine",
            "Process.PrimaryImagePath",
            "Process.PrimaryImageMd5",
            "Process.StartDateTime",
            "NetworkConnection.DestAddr",
            "NetworkConnection.DestPort",
            "RegistryKey.ProcessName",
            "RegistryKey.ProcessPrimaryImagePath",
            "RegistryKey.ValueName",
            "RegistryKey.FilePath",
            "RegistryKey.FileMd5",
            "RegistryKey.IsPersistencePoint")]
        public string QueryType { get; set; }

        [Parameter]
        public SwitchParameter CaseSensitive { get; set; }

        [Parameter(Mandatory = false)]
        public SwitchParameter ExactMatch { get; set; }

        [Parameter(Mandatory = true)]
        public string[] Query { get; set; }

        [Parameter(Mandatory = true)]
        public CyZone[] Zones { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("Windows", "MacOS")]
        public string[] OS { get; set; } = new[] { "Windows" };

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            var s = QueryType.Split('.');
            var artifact = s[0];
            var facet = s[1];
            switch (facet)
            {
                case "SHA256":
                    facet = "Sha2";
                    break;
            }
            var filtersList = new List<CyInstaQuery.CyInstaQueryFilter>();
            if (OS.Contains("Windows"))
            {
                filtersList.Add(new CyInstaQuery.CyInstaQueryFilter
                {
                    aspect = "OS",
                    value = "Windows"
                });
            }

            var iq = Connection.CreateInstaQueryAsync(Name, Description, Zones, filtersList.ToArray(), artifact, Query,
                facet, ExactMatch, CaseSensitive).WaitAndUnwrapException();
            WriteObject(iq);
        }
    }
}
