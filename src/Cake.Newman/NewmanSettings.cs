﻿using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Newman.Reporters;
using System.Linq;

namespace Cake.Newman
{
    /// <summary>
    /// Settings to control the <see cref="NewmanRunner"/>.
    /// </summary>
    public class NewmanSettings : ToolSettings
    {
        /// <summary>
        ///     An environment file path.
        /// </summary>
        /// <remarks>Environments provide a set of variables that one can use within collections.</remarks>
        public FilePath EnvironmentFile { get; set; }

        /// <summary>
        ///     An environment file path.
        /// </summary>
        /// <remarks>Data provide a set of variables for each iteration that one can use within collections.</remarks>
        public FilePath DataFile { get; set; }

        /// <summary>
        ///     File path for global variables.
        /// </summary>
        /// <remarks>
        ///     Global variables are similar to environment variables but has a lower precedence and can be overridden by
        ///     environment variables having same name.
        /// </remarks>
        public FilePath GlobalVariablesFile { get; set; }

        /// <summary>
        ///     Run requests within a particular folder in a collection.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        ///     Prevents reading of files situated outside of the working directory.
        /// </summary>
        public bool NoInsecureFileRead { get; set; }

        /// <summary>
        ///     The path to the file where Newman will output the final environment variables file before completing a run.
        /// </summary>
        public FilePath ExportEnvironmentPath { get; set; }

        /// <summary>
        ///     The path to the file where Newman will output the final global variables file before completing a run.
        /// </summary>
        public FilePath ExportGlobalsPath { get; set; }

        /// <summary>
        ///     The path to the file where Newman will output the final collection file before completing a run.
        /// </summary>
        public FilePath ExportCollectionPath { get; set; }

        /// <summary>
        ///     The time (in milliseconds) to wait for requests to return a response.
        /// </summary>
        public int RequestTimeout { get; set; }

        /// <summary>
        ///     The time (in milliseconds) to wait for script to execute.
        /// </summary>
        public int ScriptTimeout { get; set; }

        /// <summary>
        ///     Disables SSL verification checks and allows self-signed SSL certificates.
        /// </summary>
        public bool DisableStrictSSL { get; set; }

        /// <summary>
        ///     Prevents newman from automatically following 3XX redirect responses.
        /// </summary>
        public bool IgnoreRedirects { get; set; }

        /// <summary>
        ///     Specify the extent of delay between requests (in milliseconds).
        /// </summary>
        public int RequestDelay { get; set; }

        /// <summary>
        ///     Specify whether or not to stop a collection run on encountering the first error.
        /// </summary>
        public bool ExitOnFirstFailure { get; set; }

        /// <summary>
        ///     Specify whether or not to override the default exit code for the current run.
        /// </summary>
        public bool SuppressExitCode { get; set; }

        /// <summary>
        ///     Specify iteration count.
        /// </summary>
        public int IterationCount { get; set; }

        /// <summary>
        ///     Enable or Disable colored CLI output. The color value can be any of the three: 'on', 'off' or 'auto'(default).
        /// </summary>
        /// <remarks>
        /// With 'auto', Newman attempts to automatically turn color on or off based on the color support in the terminal. This behaviour can be modified by using the 'on' or 'off' value accordingly.
        /// </remarks>
        public string Color { get; set; }

        /// <summary>
        ///     Specify whether or not to force the unicode disable option. When supplied, all symbols in the output will be replaced by their plain text equivalents.
        /// </summary>
        public bool DisableUnicode { get; set; }

        /// <summary>
        ///     Show detailed information of collection run and each request sent.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        ///     Newman supports SSL client certificates, via the following CLI options
        /// </summary>
        public SSLClientCertificateSettings SSLClientCertificateSettings { get; set; }

        /// <summary>
        ///     Reporters (and any reporter-specific options) for test results
        /// </summary>
        public Dictionary<string, IReporterSettings> Reporters { get; set; } =
            new Dictionary<string, IReporterSettings>();

        /// <summary>
        ///     Builds the complete arguments for invoking newman
        /// </summary>
        /// <param name="args">The argument builder.</param>
        public void Build(ProcessArgumentBuilder args)
        {
            if (EnvironmentFile != null) args.AppendSwitchQuoted(ArgumentNames.Environment, EnvironmentFile.FullPath);
            if (DataFile != null) args.AppendSwitchQuoted(ArgumentNames.Data, DataFile.FullPath);

            if (GlobalVariablesFile != null)
                args.AppendSwitchQuoted(ArgumentNames.Globals, GlobalVariablesFile.FullPath);
            if (!string.IsNullOrWhiteSpace(Folder)) args.AppendSwitchQuoted(ArgumentNames.Folder, Folder);
            if (ExportEnvironmentPath != null)
            {
                args.AppendSwitchQuoted(ArgumentNames.ExportEnvironment, ExportEnvironmentPath.FullPath);
            }
            if (ExportGlobalsPath != null)
                args.AppendSwitchQuoted(ArgumentNames.ExportGlobals, ExportGlobalsPath.FullPath);
            if (ExportCollectionPath != null)
            {
                args.AppendSwitchQuoted(ArgumentNames.ExportCollection, ExportCollectionPath.FullPath);
            }
            if (RequestTimeout != default(int))
            {
                args.AppendSwitch(ArgumentNames.RequestTimeout, RequestTimeout.ToString());
            }
            if (ScriptTimeout != default(int))
            {
                args.AppendSwitch(ArgumentNames.ScriptTimeout, ScriptTimeout.ToString());
            }
            if (!string.IsNullOrWhiteSpace(Color)) args.AppendSwitch(ArgumentNames.Color, Color);
            if (NoInsecureFileRead) args.Append(ArgumentNames.NoInsecureFileRead);
            if (DisableStrictSSL) args.Append(ArgumentNames.Insecure);
            if (IgnoreRedirects) args.Append(ArgumentNames.IgnoreRedirects);
            if (RequestDelay != default(int)) args.AppendSwitch(ArgumentNames.RequestDelay, RequestDelay.ToString());
            if (IterationCount != default(int)) args.AppendSwitch(ArgumentNames.IterationCount, IterationCount.ToString());
            if (ExitOnFirstFailure) args.Append(ArgumentNames.Bail);
            if (SuppressExitCode) args.Append(ArgumentNames.SuppressExitCode);
            if (DisableUnicode) args.Append(ArgumentNames.DisableUnicode);
            if (Verbose) args.Append(ArgumentNames.Verbose);
            if (Reporters.Any())
            {
                args.AppendSwitch(ArgumentNames.Reporters,
                    string.Join(",", Reporters.Keys.Select(k => k.Trim())));
                foreach (var reporter in Reporters)
                {
                    reporter.Value?.RenderOptions(args);
                }
            }
            if (SSLClientCertificateSettings != null)
            {
                if (SSLClientCertificateSettings.Cert != null)
                    args.AppendSwitchQuoted(ArgumentNames.SslClientCert, SSLClientCertificateSettings.Cert.FullPath);
                if (SSLClientCertificateSettings.Key != null)
                    args.AppendSwitchQuoted(ArgumentNames.SslClientKey, SSLClientCertificateSettings.Key.FullPath);
                if (!string.IsNullOrWhiteSpace(SSLClientCertificateSettings.Passphrase))
                    args.AppendSwitchQuotedSecret(ArgumentNames.SslClientPassphrase, SSLClientCertificateSettings.Passphrase);
            }
        }
    }
}