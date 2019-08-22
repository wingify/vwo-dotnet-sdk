using System;
using Identifiable;

namespace VWOSdk
{
    internal class UuidV5Helper
    {
        /// <summary>
        /// The namespace for fully-qualified domain names (from RFC 4122, Appendix C).
        /// </summary>
        private static readonly Guid DnsNamespace = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        /// The namespace for URLs (from RFC 4122, Appendix C).
        /// </summary>
        private static readonly Guid UrlNamespace = new Guid("6ba7b811-9dad-11d1-80b4-00c04fd430c8");

        /// <summary>
        /// The namespace for ISO OIDs (from RFC 4122, Appendix C).
        /// </summary>
        private static readonly Guid IsoOidNamespace = new Guid("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
        private static readonly string file = typeof(UuidV5Helper).FullName;
        private const string GUID_FORMAT = "N";
        private const string VWO_NAMESPACE_URL = "https://vwo.com";

        public static string Compute(long accountId, string userId)
        {
            var accountIdAsString = accountId.ToString();
            var vwoNamespaceGuid = Identifiable.NamedGuid.Compute(NamedGuidAlgorithm.SHA1, UrlNamespace, VWO_NAMESPACE_URL);
            var accountIdGuid = NamedGuid.Compute(NamedGuidAlgorithm.SHA1, vwoNamespaceGuid, accountIdAsString);
            var userIdGuid = NamedGuid.Compute(NamedGuidAlgorithm.SHA1, accountIdGuid, userId);
            var uuid = userIdGuid.ToString(GUID_FORMAT).ToUpper();
            LogDebugMessage.UuidForUser(file, userId, accountId, uuid);
            return uuid;
        }
    }
}
