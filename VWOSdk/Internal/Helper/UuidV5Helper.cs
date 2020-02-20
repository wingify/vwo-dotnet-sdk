#pragma warning disable 1587
/**
 * Copyright 2019-2020 Wingify Software Pvt. Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
#pragma warning restore 1587

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
