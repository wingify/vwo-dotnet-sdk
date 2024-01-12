#pragma warning disable 1587
/**
 * Copyright 2019-2021 Wingify Software Pvt. Ltd.
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

using System;

namespace VWOSdk
{
    public interface IRedisUserStorageService
    {
        /// <summary>
        /// Initialize the Redis User Storage with the provided URL, UserId, and password.
        /// </summary>
        /// <param name="url">The Redis server URL.</param>
        /// <param name="userId">The user ID.</param>
        /// <param name="password">The password for authentication (if required).</param>
       // void Init(RedisConfig redisConfig);

        /// <summary>
        /// Get data from Redis for a specific user and campaign key.
        /// </summary>
        /// <param name="userId">The user ID for which data is requested.</param>
        /// <param name="campaignKey">The campaign key for data lookup.</param>
        /// <returns>The data retrieved from Redis.</returns>
        string Get(string userId, string campaignKey);

        /// <summary>
        /// Set data in Redis for a specific user and campaign key.
        /// </summary>
        /// <param name="userId">The user ID for which data is set.</param>
        /// <param name="campaignKey">The campaign key for data storage.</param>
        /// <param name="data">The data to be stored in Redis.</param>
        void Set(UserStorageMap userStorageMap);
    }
}
