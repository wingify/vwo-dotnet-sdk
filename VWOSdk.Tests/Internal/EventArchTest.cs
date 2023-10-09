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

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Reflection;
using Xunit;
namespace VWOSdk.Tests
{
    public class EventArchTest
    {
        internal static Settings settings = new FileReaderApiCaller("AB_TRAFFIC_50_WEIGHT_50_50 ").GetJsonContent<Settings>();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        private static IVWOClient vwoInstance { get; set; }
        private static readonly string sdkVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        [Fact]
        public void TrackUserPayloadTest()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackUser payload and queryParams for enabled event arch");
            string payLoad = ServerSideVerb.GetTrackUserArchEnabledPayload("Ashley", settings.AccountId, settings.SdkKey, sdkVersion, 20, 3);
            Dictionary<string, int> usageStats = new Dictionary<string, int>();
            usageStats.Add("_l", 1);
            usageStats.Add("cl", 1);
            usageStats.Add("ll", 1);
            Dictionary<string, dynamic> queryParams = ServerSideVerb.getEventArchQueryParams(settings.AccountId, settings.SdkKey, usageStats);

            Assert.True(queryParams.TryGetValue("a", out dynamic a));
            Assert.NotNull(a);
            Assert.Equal(settings.AccountId.ToString(), a.ToString());
            Assert.True(queryParams.TryGetValue("en", out dynamic en));
            Assert.NotNull(en);
            Assert.True(queryParams.TryGetValue("eTime", out dynamic eTime));
            Assert.NotNull(eTime);
            Assert.True(queryParams.TryGetValue("random", out dynamic random));
            Assert.NotNull(random);
            Assert.True(queryParams.TryGetValue("env", out dynamic env));
            Assert.NotNull(env);
            Assert.True(queryParams.TryGetValue("_l", out dynamic _l));
            Assert.NotNull(_l);
            Assert.True(queryParams.TryGetValue("cl", out dynamic cl));
            Assert.NotNull(cl);
            Assert.True(queryParams.TryGetValue("ll", out dynamic ll));
            Assert.NotNull(ll);
            Assert.True(typeof(string).IsInstanceOfType(a));
            Assert.True(typeof(string).IsInstanceOfType(en));
            Assert.True(typeof(long).IsInstanceOfType(long.Parse(eTime)));
            Assert.True(typeof(double).IsInstanceOfType(double.Parse(random)));
            Assert.True(typeof(string).IsInstanceOfType(env));
            Assert.True(typeof(int).IsInstanceOfType(int.Parse(_l)));
            Assert.True(typeof(int).IsInstanceOfType(int.Parse(cl)));
            Assert.True(typeof(int).IsInstanceOfType(int.Parse(ll)));
            var parsed = JObject.Parse(payLoad);
            Assert.True(parsed.HasValues);
            Assert.True(parsed.SelectToken("d").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.msgId").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visId").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.sessionId").Value<long>()));
            Assert.True(parsed.SelectToken("d.visitor").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d.visitor").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.vwo_fs_environment").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.name").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.event.time").Value<long>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkName").Value<string>()));
            Assert.True(parsed.SelectToken("d.event").HasValues && typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkVersion").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_envKey").Value<string>()));
            Assert.True(typeof(int).IsInstanceOfType(parsed.SelectToken("d.event.props.id").Value<int>()));
            Assert.True(typeof(int).IsInstanceOfType(parsed.SelectToken("d.event.props.variation").Value<int>()));
            Assert.True(typeof(int).IsInstanceOfType(parsed.SelectToken("d.event.props.isFirst").Value<int>()));
        }
        [Fact]
        public void trackGoalPayloadTest()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "TrackGoal payload and queryParams for enabled event arch");
            Dictionary<string, int> metricMap = new Dictionary<string, int>();
            metricMap.Add("20", 20);
            metricMap.Add("10", 30);
            metricMap.Add("50", 40);
            string payLoad = ServerSideVerb.GetGoalArchEnabledPayload("Ashley", settings.AccountId, settings.SdkKey, sdkVersion, metricMap, new List<string>() { "revenue" }, "300", "goalIdentifier");
            Dictionary<string, dynamic> queryParams = ServerSideVerb.getEventArchTrackGoalParams(settings.AccountId, settings.SdkKey, "goalIdentifier");
            Assert.True(queryParams.TryGetValue("a", out dynamic a));
            Assert.NotNull(a);
            Assert.Equal(settings.AccountId.ToString(), a.ToString());
            Assert.True(queryParams.TryGetValue("en", out dynamic en));
            Assert.NotNull(en);
            Assert.True(queryParams.TryGetValue("eTime", out dynamic eTime));
            Assert.NotNull(eTime);
            Assert.True(queryParams.TryGetValue("random", out dynamic random));
            Assert.NotNull(random);
            Assert.True(queryParams.TryGetValue("env", out dynamic env));
            Assert.NotNull(env);
            Assert.True(typeof(string).IsInstanceOfType(a));
            Assert.True(typeof(string).IsInstanceOfType(en));
            Assert.True(typeof(long).IsInstanceOfType(long.Parse(eTime)));
            Assert.True(typeof(double).IsInstanceOfType(double.Parse(random)));
            Assert.True(typeof(string).IsInstanceOfType(env));
            var parsed = JObject.Parse(payLoad);
            Assert.True(parsed.HasValues);
            Assert.True(parsed.SelectToken("d").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.msgId").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visId").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.sessionId").Value<long>()));
            Assert.True(parsed.SelectToken("d.visitor").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d.visitor").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.vwo_fs_environment").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.name").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.event.time").Value<long>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkName").Value<string>()));
            Assert.True(parsed.SelectToken("d.event").HasValues && typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkVersion").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_envKey").Value<string>()));
            Assert.True(parsed.SelectToken("d.event.props.vwoMeta").HasValues && typeof(int).IsInstanceOfType(parsed.SelectToken("d.event.props.vwoMeta.revenue").Value<int>()));
            Assert.True(parsed.SelectToken("d.event.props.vwoMeta.metric.id_20").Value<JArray>().GetType().Name == "JArray");
            Assert.True(typeof(bool).IsInstanceOfType(parsed.SelectToken("d.event.props.isCustomEvent").Value<bool>()));
        }
        [Fact]
        public void pushPayloadTest()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "Push payload and queryParams for enabled event arch");
            string payLoad = ServerSideVerb.GetPushTagsArchEnabledPayload("Ashley", settings.AccountId, settings.SdkKey, sdkVersion, new Dictionary<string, string>() { { "tagKey", "TagValue" } });
            Dictionary<string, dynamic> queryParams = ServerSideVerb.getEventArchPushParams(settings.AccountId, settings.SdkKey);
            Assert.True(queryParams.TryGetValue("a", out dynamic a));
            Assert.NotNull(a);
            Assert.Equal(settings.AccountId.ToString(), a.ToString());
            Assert.True(queryParams.TryGetValue("en", out dynamic en));
            Assert.NotNull(en);
            Assert.True(queryParams.TryGetValue("eTime", out dynamic eTime));
            Assert.NotNull(eTime);
            Assert.True(queryParams.TryGetValue("random", out dynamic random));
            Assert.NotNull(random);
            Assert.True(queryParams.TryGetValue("env", out dynamic env));
            Assert.NotNull(env);
            Assert.True(typeof(string).IsInstanceOfType(a));
            Assert.True(typeof(string).IsInstanceOfType(en));
            Assert.True(typeof(long).IsInstanceOfType(long.Parse(eTime)));
            Assert.True(typeof(double).IsInstanceOfType(double.Parse(random)));
            Assert.True(typeof(string).IsInstanceOfType(env));
            var parsed = JObject.Parse(payLoad);
            Assert.True(parsed.HasValues);
            Assert.True(parsed.SelectToken("d").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.msgId").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visId").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.sessionId").Value<long>()));
            Assert.True(parsed.SelectToken("d.visitor").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d.visitor").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.vwo_fs_environment").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.name").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.event.time").Value<long>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkName").Value<string>()));
            Assert.True(parsed.SelectToken("d.event").HasValues && typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkVersion").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_envKey").Value<string>()));
            Assert.True(typeof(bool).IsInstanceOfType(parsed.SelectToken("d.event.props.isCustomEvent").Value<bool>()));
        }
        [Fact]
        public void pushPayloadWithMultipleCDTest()
        {
            VWO.Configure(new Validator());
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "Push payload and queryParams with multiple CD for enabled event arch");
            string payLoad = ServerSideVerb.GetPushTagsArchEnabledPayload("Ashley", settings.AccountId, settings.SdkKey, sdkVersion,
                new Dictionary<string, string>() {
                { "string", "string" },
                { "int", "20" },
                { "double", "20.34" },
                { "boolean", "false" }
                });
            Dictionary<string, dynamic> queryParams = ServerSideVerb.getEventArchPushParams(settings.AccountId, settings.SdkKey);
            Assert.True(queryParams.TryGetValue("a", out dynamic a));
            Assert.NotNull(a);
            Assert.Equal(settings.AccountId.ToString(), a.ToString());
            Assert.True(queryParams.TryGetValue("en", out dynamic en));
            Assert.NotNull(en);
            Assert.True(queryParams.TryGetValue("eTime", out dynamic eTime));
            Assert.NotNull(eTime);
            Assert.True(queryParams.TryGetValue("random", out dynamic random));
            Assert.NotNull(random);
            Assert.True(queryParams.TryGetValue("env", out dynamic env));
            Assert.NotNull(env);
            Assert.True(typeof(string).IsInstanceOfType(a));
            Assert.True(typeof(string).IsInstanceOfType(en));
            Assert.True(typeof(long).IsInstanceOfType(long.Parse(eTime)));
            Assert.True(typeof(double).IsInstanceOfType(double.Parse(random)));
            Assert.True(typeof(string).IsInstanceOfType(env));
            var parsed = JObject.Parse(payLoad);
            Assert.True(parsed.HasValues);
            Assert.True(parsed.SelectToken("d").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.msgId").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visId").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.sessionId").Value<long>()));
            Assert.True(parsed.SelectToken("d.visitor").HasValues && typeof(JObject).IsInstanceOfType(parsed.SelectToken("d.visitor").Value<JObject>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.string").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.int").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.double").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.boolean").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.visitor.props.vwo_fs_environment").Value<string>()));
            Assert.Null(parsed.SelectToken("d.visitor.props.tagKey"));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.name").Value<string>()));
            Assert.True(typeof(long).IsInstanceOfType(parsed.SelectToken("d.event.time").Value<long>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkName").Value<string>()));
            Assert.True(parsed.SelectToken("d.event").HasValues && typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_sdkVersion").Value<string>()));
            Assert.True(typeof(string).IsInstanceOfType(parsed.SelectToken("d.event.props.vwo_envKey").Value<string>()));
            Assert.True(typeof(bool).IsInstanceOfType(parsed.SelectToken("d.event.props.isCustomEvent").Value<bool>()));
        }
    }
}
