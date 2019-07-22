using System;
using System.Management.Automation;
using System.Net;
using System.Security;
using System.Threading.Tasks;
using TietzeIO.CyAPI;
using TietzeIO.CyAPI.Configuration;
using TietzeIO.CyAPI.Configuration.Plaintext;
using TietzeIO.CyAPI.Exceptions;
using static TietzeIO.CyAPI.ApiV2;

namespace TietzeIO.CyShell.Cmdlets.Base
{
    /// <summary>
    /// Base class for cmdlets.
    /// 
    /// Provides common logging capability that can be attached to APIv2.
    /// </summary>
    public abstract class CyCmdlet : PSCmdlet
    {
        internal PowershellThreadSafeVerboseLogger Logger { get; set; }

        protected CyCmdlet()
        {
            Logger = new PowershellThreadSafeVerboseLogger(this);
        }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            Logger.FlushLogsToPowershellConsole();
        }

        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            Logger.FlushLogsToPowershellConsole();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
            Logger.FlushLogsToPowershellConsole();
        }

        protected override void StopProcessing()
        {
            WriteLine("Called StopProcessing");
            base.StopProcessing();
            Logger.FlushLogsToPowershellConsole();
        }

        private const ConsoleColor HIGHLIGHT = ConsoleColor.Green;

        public void WriteHL(string message)
        {
            var old = Host.UI.RawUI.ForegroundColor;
            Host.UI.RawUI.ForegroundColor = HIGHLIGHT;
            Write(message);
            Host.UI.RawUI.ForegroundColor = old;
        }

        public void Write(string message)
        {
            Host.UI.Write(message);
        }

        public void WriteLine(string message = "")
        {
            Host.UI.WriteLine(message);
        }

        public void WriteLineHL(string message = "")
        {
            var old = Host.UI.RawUI.ForegroundColor;
            Host.UI.RawUI.ForegroundColor = HIGHLIGHT;
            WriteLine(message);
            Host.UI.RawUI.ForegroundColor = old;
        }

        /// <summary>
        /// Returns a SecureString from a field accepting credentials in various formats.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        protected SecureString ConvertToSecureString(string field, PSObject secret)
        {
            if (secret == null) return null;
            var apiSecret = new SecureString();

            if (secret.ImmediateBaseObject is SecureString)
            {
                WriteVerbose($"Detected {field} as SecureString");
                apiSecret = secret.ImmediateBaseObject as SecureString;
            }
            else if (secret.ImmediateBaseObject is string)
            {
                WriteVerbose($"Detected {field} as string");
                var s = secret.ImmediateBaseObject as string;
                foreach (var t in s)
                    apiSecret.AppendChar(t);
            }
            else if (secret.ImmediateBaseObject is NetworkCredential)
            {
                WriteVerbose($"Detected {field} as NetworkCredential");
                apiSecret = (secret.ImmediateBaseObject as NetworkCredential).SecurePassword;
            }

            return apiSecret;
        }

        /// <summary>
        /// Attempts to perform the supplied action, while suppressing exceptions for certain HTTP status code.
        /// When one of the suppressed HTTP statuses is encountered, it will log the message supplied.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        /// <param name="tolerableHttpStatusCodes"></param>
        protected void AttemptActionAndSoftFail(Action action, string[] messages, HttpStatusCode[] tolerableHttpStatusCodes)
        {
            try
            {
                action.Invoke();
            }
            catch (ApiV2Exception ex)
            {
                for (int i = 0; i < messages.Length; i++)
                {
                    if (ex.Status.HasValue && (ex.Status.Value == tolerableHttpStatusCodes[i]))
                    {
                        WriteVerbose($"Soft fail: {messages[i]} - {ex.Message}");
                        return;
                    }
                }
                throw ex;
            }
        }

        /// <summary>
        /// Attempts to perform the supplied task, while suppressing exceptions for certain HTTP status code.
        /// When one of the suppressed HTTP statuses is encountered, it will log the message supplied.
        /// </summary>
        /// <param name="task"></param>
        /// <param name="message"></param>
        /// <param name="tolerableHttpStatusCodes"></param>
        protected async Task<T> AttemptAndSoftFail<T>(Task<T> task, string[] messages, HttpStatusCode[] tolerableHttpStatusCodes) where T : class
        {
            try
            {
                return await task.ConfigureAwait(false);
            }
            catch (ApiV2Exception ex)
            {
                for (int i = 0; i < messages.Length; i++)
                {
                    if (ex.Status.HasValue && (ex.Status.Value == tolerableHttpStatusCodes[i]))
                    {
                        Logger.LogVerbose($"Soft fail: {messages[i]} - {ex.Message}");
                        return null;
                    }
                }

                throw ex;
            }
        }
    }
}