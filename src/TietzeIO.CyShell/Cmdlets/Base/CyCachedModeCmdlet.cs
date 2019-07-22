using System;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    /// <summary>
    /// A cmdlet that requires the cache to be warm.
    /// </summary>
    public abstract class CyCachedModeCmdlet : CyApiCmdlet
    {
        protected bool RequireOpticsCache { get; set; } = true;
        protected bool RequireProtectCache { get; set; } = true;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            if (!Api.CacheMode ||
                !((!RequireOpticsCache || (RequireOpticsCache && Api.IncludeOpticsData))) ||
                !((!RequireProtectCache || (RequireProtectCache && Api.IncludeProtectData))))
            {
                WriteLine("Required cache(s) not initialized. Do you want to initialize cache(s) now (Y/N)?");
                var answer = Host.UI.ReadLine();
                if (answer.StartsWith("Y", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    Api.IncludeProtectData |= RequireProtectCache;
                    Api.IncludeOpticsData |= RequireOpticsCache;
                    Api.RefreshCache(Connection, this);
                    Api.CacheMode = true;
                }
                else
                {
                    throw new Exception("Cache mode not initialized. This cmdlet requires cache mode to run.");
                }
            }
        }
    }
}