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

#pragma warning restore 1587
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;


namespace VWOSdk
{
    internal class RedisUserStorageService : IUserStorageService
    {
        private StackExchange.Redis.ConnectionMultiplexer redisConnection;
        private StackExchange.Redis.IDatabase redisDatabase;

        public RedisUserStorageService(RedisConfig redisConfig)
        {
            try
            {
                // Initialize Redis connection here
                StackExchange.Redis.ConfigurationOptions options = StackExchange.Redis.ConfigurationOptions.Parse(redisConfig.Url);
                options.Password = redisConfig.Password;
                options.AbortOnConnectFail = false;

                this.redisConnection = StackExchange.Redis.ConnectionMultiplexer.Connect(options);
                this.redisDatabase = this.redisConnection.GetDatabase();
                
            }
            catch (Exception ex)
            {
                // Handle the exception, e.g., log it or take appropriate action
                LogErrorMessage.UnableToConnectToRedis(typeof(IVWOClient).FullName, ex.Message);
            }

        }

        public UserStorageMap Get(string userId, string campaignKey)
        {
            if (this.redisDatabase != null)
            {
                try
                {
                    string key = $"{campaignKey}:{userId}";
                    string result = this.redisDatabase.StringGet(key);

                    if (!string.IsNullOrEmpty(result))
                    {
                        return Newtonsoft.Json.JsonConvert.DeserializeObject<UserStorageMap>(result);
                    }
                }
                catch (Exception ex)
                {
                    LogErrorMessage.UnableToRetrieveDataFromRedis(typeof(IVWOClient).FullName, ex.Message);
                    throw ex;
                }
            }
            else
            {
                LogErrorMessage.RedisClientError(typeof(IVWOClient).FullName);
            }

            return null;
        }

        public void Set(UserStorageMap userStorageMap)
        {
            if (this.redisDatabase != null)
            {
                string key = $"{userStorageMap.CampaignKey}:{userStorageMap.UserId}";
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(userStorageMap);

                try
                {
                    this.redisDatabase.StringSet(key, data);
                }
                catch (Exception ex)
                {
                    LogErrorMessage.UnableToSetDataToRedis(typeof(IVWOClient).FullName, ex.Message);
                }
            }
            else
            {
                LogErrorMessage.RedisClientError(typeof(IVWOClient).FullName);
            }
        }
    }
}