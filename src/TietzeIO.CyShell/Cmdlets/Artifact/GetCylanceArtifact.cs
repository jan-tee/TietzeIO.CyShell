using System;
using System.Collections.Generic;
using System.Management.Automation;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Entities;
using TietzeIO.CyAPI.Entities.Optics;
using TietzeIO.CyShell.Cmdlets.Base;

namespace TietzeIO.CyShell.Cmdlets.Artifact
{
    [Cmdlet(VerbsCommon.Get, "CylanceArtifact")]
    [OutputType(typeof(CyDeviceMetaData))]
    public class GetCylanceArtifact : CyApiCmdlet // , IDynamicParameters
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true, ParameterSetName = "ByDetection")]
        public CyDetection Detection { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("Instigating Process", "Instigating Process Image File", "Target Process", "Target Process Image File", "Instigating Process Owner", "Target Process Owner", "Target File")]
        public string Source { get; set; }

        [Parameter(Mandatory = false)]
        [ValidateSet("File", "Process", "User")]
        public string ArtifactType { get; set; }

        //[Parameter(Mandatory = false)]
        //[ValidateSet("Name", "Path", "CommandLine", "Domain", "Sha256Hash", "Md5Hash", "Size", "SuspectedFileType")]
        //public string[] Facet { get; set; }

        //private PowershellDynamicParameter PhoneticIdParam;
        //private PowershellDynamicParameter DeviceNameParam;
        //private PowershellDynamicParameter RuleNameParam;
        //public object GetDynamicParameters()
        //{
        //    if (!API.ShellMode) return null;

        //    PhoneticIdParam = new PowershellDynamicParameter("PhoneticId", (from d in API.Detections select d.PhoneticId).ToArray());
        //    PhoneticIdParam.ParameterSetName = "ByPhoneticId";
        //    PhoneticIdParam.AllowMultiple = true;
        //    PhoneticIdParam.Position = 1;

        //    DeviceNameParam = new PowershellDynamicParameter("Device", (from d in API.Devices select d.name).ToArray());
        //    DeviceNameParam.ParameterSetName = "ByDevice";
        //    DeviceNameParam.AllowMultiple = true;
        //    DeviceNameParam.Position = 1;

        //    RuleNameParam = new PowershellDynamicParameter("Rule", (from d in API.Detections select d.DetectionDescription).Distinct().OrderBy(d => d).ToArray());
        //    RuleNameParam.ParameterSetName = "ByRule";
        //    RuleNameParam.AllowMultiple = true;
        //    RuleNameParam.Position = 1;

        //    var dynamicParams = PhoneticIdParam.GetParamDictionary();
        //    DeviceNameParam.AddToParamDictionary(dynamicParams);
        //    RuleNameParam.AddToParamDictionary(dynamicParams);
        //    return dynamicParams;
        //}

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            //switch (ParameterSetName)
            //{
            //    case "ByPhoneticId":
            //        var phoneticIds = PhoneticIdParam.GetValues(this);
            //        foreach (var value in phoneticIds)
            //        {
            //            var id = (from d in API.Detections where value.Equals(d.PhoneticId, StringComparison.InvariantCultureIgnoreCase) select d.id).First();
            //            var detection = Connection.RequestDetection(id);
            //            if (null != detection) WriteObject(detection);
            //        }
            //        break;
            //    case "ByDevice":
            //        var deviceNames = DeviceNameParam.GetValues(this);
            //        foreach (var value in deviceNames)
            //        {
            //            var detections = from d in API.Detections where value.Equals(d.Device.name, StringComparison.InvariantCultureIgnoreCase) select d.id;
            //            if (detections != null)
            //            {
            //                foreach (var d in detections)
            //                {
            //                    var detection = Connection.RequestDetection(d);
            //                    if (null != detection) WriteObject(detection);
            //                }
            //            }
            //        }
            //        break;
            //    case "ByRule":
            //        var ruleNames = RuleNameParam.GetValues(this);
            //        foreach (var value in ruleNames)
            //        {
            //            var detections = from d in API.Detections where value.Equals(d.DetectionDescription, StringComparison.InvariantCultureIgnoreCase) select d.id;
            //            if (detections != null)
            //            {
            //                foreach (var d in detections)
            //                {
            //                    var detection = Connection.RequestDetection(d);
            //                    if (null != detection) WriteObject(detection);
            //                }
            //            }
            //        }
            //        break;
            //}
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            switch (ParameterSetName)
            {
                case "ByDetection":
                    if (Detection != null)
                    {
                        foreach (var stateArtifacts in Detection.ArtifactsOfInterest)
                        {
                            string state = stateArtifacts.Key;
                            List<CyDetection.CyArtifactOfInterest> aois = stateArtifacts.Value;
                            foreach (var aoi in aois)
                            {
                                var source = aoi.Source;
                                var type = aoi.Artifact.Type;
                                var artifact = Detection.ResolveArtifactReference(aoi.Artifact.Uid);

                                // now decide!
                                bool includeInResults = true;
                                if (null != Source) includeInResults &= Source.Equals(source, StringComparison.InvariantCultureIgnoreCase);
                                if (null != ArtifactType) includeInResults &= ArtifactType.Equals(type, StringComparison.InvariantCultureIgnoreCase);

                                if (includeInResults)
                                {
                                    ////if (null != Facet)
                                    ////{
                                    ////    new PSCustomObject().
                                    ////    // output facet only
                                    ////    switch (Facet)
                                    ////    {
                                    ////        case "Name":
                                    ////            WriteObject(artifact.Name);
                                    ////            break;
                                    ////        case "Path":
                                    ////            WriteObject(artifact.Path);
                                    ////            break;
                                    ////        case "CommandLine":
                                    ////            WriteObject(artifact.CommandLine);
                                    ////            break;
                                    ////        case "Domain":
                                    ////            WriteObject(artifact.Domain);
                                    ////            break;
                                    ////        case "Sha256Hash":
                                    ////            WriteObject(artifact.Sha256Hash);
                                    ////            break;
                                    ////        case "Md5Hash":
                                    ////            WriteObject(artifact.Md5Hash);
                                    ////            break;
                                    ////        case "Size":
                                    ////            WriteObject(artifact.Size);
                                    ////            break;
                                    ////        case "SuspectedFileType":
                                    ////            WriteObject(artifact.SuspectedFileType);
                                    ////            break;
                                    ////    }
                                    ////}
                                    ////else
                                    //{
                                    // output whole artifact object
                                    WriteObject(artifact);
                                }
                            }

                        }
                    }
                    break;
            }
        }
    }
}
