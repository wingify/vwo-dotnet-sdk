

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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace VWOSdk
{
    internal class HttpRequestBuilder
    {

        /// <summary>
        /// Params For Tracking User.
        /// </summary>
        public static IDictionary<string, dynamic> EventForTrackingUser(long accountId, int campaignId, int variationId, string userId,  bool isDevelopmentMode)
        {

            BuildQueryParams requestParams =
            BuildQueryParams.Builder.getInstance()
                     //.withAp()
                     //.withEd()
                    .withMinifiedCampaignId(campaignId)
                    .withMinifiedVariationId(variationId)
                    .withMinifiedEventType((int)EVENT_TYPES.TRACK_USER)
                    .withSid(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    .withUuid(accountId, userId)
                    .build();

            IDictionary<string, dynamic> map = requestParams.removeNullValues(requestParams);



            return map;
        }
        /// <summary>
        /// Params For Tracking Goal.
        /// </summary>
        public static IDictionary<string, dynamic> EventForTrackingGoal(long accountId, int campaignId, int variationId, string userId,
            int goalId,  string revenueValue, bool isDevelopmentMode)
        {


            BuildQueryParams requestParams = BuildQueryParams.Builder.getInstance()
                  // .withAp()
                    // .withEd()
                .withMinifiedCampaignId(campaignId)
                .withMinifiedVariationId(variationId)
                .withMinifiedEventType((int)EVENT_TYPES.TRACK_GOAL)
                .withMinifiedGoalId(goalId)
                .withRevenue(revenueValue)
                .withSid(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .withUuid(accountId, userId)
                .build();

            IDictionary<string, dynamic> map = requestParams.removeNullValues(requestParams);
            return map;
        }
        /// <summary>
        /// Params For Push Tags.
        /// </summary>
        public static IDictionary<string, dynamic> EventForPushTags(long accountId, string tagKey, string tagValue, string userId, bool isDevelopmentMode)
        {

            BuildQueryParams requestParams =
            BuildQueryParams.Builder.getInstance()
              // .withAp()
                   //  .withEd()
                    .withMinifiedEventType((int)EVENT_TYPES.PUSH)
                    .withMinifiedTags(tagKey, tagValue)
                    .withSid(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                    .withUuid(accountId, userId)
                    .build();
            IDictionary<string, dynamic> map = requestParams.removeNullValues(requestParams);
            return map;
        }

        /// <summary>
        ///Convert Queue data As Json String.
        /// </summary>
        public static string GetJsonString( Queue<IDictionary<string, dynamic>> batchQueue)
        {
            string jsonString = JsonConvert.SerializeObject(batchQueue);
            jsonString = "{\"ev\":" + jsonString + "}";

            return jsonString;

        }

    }
}
