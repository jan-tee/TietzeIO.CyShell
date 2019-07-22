using System.Management.Automation;
using System.Threading;
using TietzeIO.CyAPI;
using TietzeIO.CyShell.Session;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    /// <summary>
    /// Base class for PS Cmdlets that depend on an API connection.
    /// 
    /// Provides:
    /// - A logger for APIv2 (automatically attached)
    /// - Cancellation Token for async operations
    /// </summary>
    public abstract class CyApiCmdlet : CyCmdlet
    {
        private ApiConnectionHandle _api;

        /// <summary>
        /// Contains either a parameter from the Powershell session, or a globally cached version of an API session descriptor
        /// </summary>
        [Parameter]
        public ApiConnectionHandle Api
        {
            get
            {
                if (_api != null) return _api;
                return ApiConnectionHandle.Global;
            }
            set
            {
                _api = value;
            }
        }

        protected CancellationToken Cancellation;
        protected CancellationTokenSource CancellationSource = new CancellationTokenSource();

        private ApiV2 _connection;
        /// <summary>
        /// Returns an APIv2 connection object for this session.
        /// </summary>
        public ApiV2 Connection
        {
            get
            {
                if (null == _connection)
                {
                    _connection = new ApiV2(Api.Session)
                    {
                        Cancellation = CancellationSource.Token
                    };
                }
                return _connection;
            }
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
            CancellationSource.Cancel();
            Logger.FlushLogsToPowershellConsole();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            Logger.FlushLogsToPowershellConsole();
        }
    }
}