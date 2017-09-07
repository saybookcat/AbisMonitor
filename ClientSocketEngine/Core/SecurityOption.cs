using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace ClientSocketEngine.Core
{
    /// <summary>
    /// Security options
    /// </summary>
    public class SecurityOption
    {
        /// <summary>
        /// The SslProtocols want to be enabled
        /// </summary>
        public SslProtocols EnabledSslProtocols { get; set; }

        /// <summary>
        /// Client X509 certificates
        /// </summary>
        public X509CertificateCollection Certificates { get; set; }

        /// <summary>
        /// Whether allow untrusted certificate
        /// </summary>
        public bool AllowUnstrustedCertificate { get; set; }

        /// <summary>
        /// Whether allow the certificate whose name doesn't match current remote endpoint's host name
        /// </summary>
        public bool AllowNameMismatchCertificate { get; set; }

        public SecurityOption()
            : this(GetDefaultProtocol(), new System.Security.Cryptography.X509Certificates.X509CertificateCollection())
        {

        }

        public SecurityOption(System.Security.Authentication.SslProtocols enabledSslProtocols)
            : this(enabledSslProtocols, new System.Security.Cryptography.X509Certificates.X509CertificateCollection())
        {

        }

        public SecurityOption(SslProtocols enabledSslProtocols, X509Certificate certificate)
            : this(enabledSslProtocols, new X509CertificateCollection(new X509Certificate[] { certificate }))
        {

        }

        public SecurityOption(SslProtocols enabledSslProtocols, X509CertificateCollection certificates)
        {
            EnabledSslProtocols = enabledSslProtocols;
            Certificates = certificates;
        }

        private static SslProtocols GetDefaultProtocol()
        {
#if NETSTANDARD
            return SslProtocols.Tls11 | SslProtocols.Tls12;
#else
            return SslProtocols.Default;
#endif
        }
    }
}