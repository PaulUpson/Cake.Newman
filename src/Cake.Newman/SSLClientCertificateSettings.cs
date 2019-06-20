using Cake.Core;
using Cake.Core.IO;

namespace Cake.Newman
{
    /// <summary>
    ///     Newman supports SSL client certificates, via the following CLI options
    /// </summary>
    public class SSLClientCertificateSettings
    {
        /// <summary>The path to the public client certificate file. </summary>
        public FilePath Cert { get; set; }
        /// <summary>The path to the private client key (optional). </summary>
        public FilePath Key { get; set; }
        /// <summary>The secret passphrase used to protect the private client key (optional). </summary>
        public string Passphrase { get; set; }
    }
}