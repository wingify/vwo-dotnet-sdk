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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace VWOSdk.Tests
{
    public class MEGTests
    {
        internal static Settings settings = new FileReaderApiCaller("SampleGroupSettingsFile").GetJsonContent<Settings>();
        internal static Settings settingsFileNewMEG = new FileReaderApiCaller("NewMEGSettingsFile1").GetJsonContent<Settings>();
        internal static ILogWriter Logger { get; set; } = new DefaultLogWriter();
        private static IVWOClient vwoInstance { get; set; }
        [Fact]
        public void WhitelistingPassedForCalledCampaign()
        {
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "should return a variation as whitelisting is satisfied for the called campaign");
            var Campaigns = settings.getCampaigns();
            var calledCampaignKey = Campaigns[2].Key;
            var otherCampaignKey = Campaigns[3].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
            {{
                "variationTargetingVariables", new Dictionary<string, dynamic>()
                {
                    {
                        "chrome", false
                    }
                }
            }};
            var variationName = vwoInstance.Activate(calledCampaignKey, "Ashley", Options);
            Assert.Equal("Variation-1", variationName);
            variationName = vwoInstance.Activate(otherCampaignKey, "Ashley", Options);
            Assert.Null(variationName);
        }
        [Fact]
        public void whitelistingFailedForCalledCampaign()
        {
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "should return null as other campaign satisfies the whitelisting");
            var Campaigns = settings.getCampaigns();
            var calledCampaignKey = Campaigns[3].Key;
            var otherCampaignKey = Campaigns[2].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
            {{
                "variationTargetingVariables", new Dictionary<string, dynamic>()
                {
                    {
                        "chrome", false
                    }
                }
            }};
            var variationName = vwoInstance.Activate(calledCampaignKey, "Ashley", Options);
            Assert.Null(variationName);
            variationName = vwoInstance.Activate(otherCampaignKey, "Ashley", Options);
            Assert.Equal("Variation-1", variationName);

        }
        [Fact]
        public void storagePassedForCalledCampaign()
        {
            UserStorageService._userStorageMap = null;
            vwoInstance = VWO.Launch(settings, true, userStorageService: new UserStorageService());
            Logger.WriteLog(LogLevel.DEBUG, "should return variation as storage is satisfied for the called campaign");
            var Campaigns = settings.getCampaigns();
            var campaignKey = Campaigns[2].Key;
            var otherCampaignKey = Campaigns[3].Key;
            Assert.Equal("0", UserStorageService.getStorage().Count.ToString());
            string variation = vwoInstance.Activate(campaignKey, "Ashley");
            Assert.Equal("Control", variation);
            Assert.Equal("1", UserStorageService.getStorage().Count.ToString());
            variation = vwoInstance.Activate(campaignKey, "Ashley");
            string variationName = vwoInstance.GetVariationName(campaignKey, "Ashley");
            Assert.Equal("Control", variation);
            Assert.Equal("Control", variationName);
            Assert.Equal("1", UserStorageService.getStorage().Count.ToString());
            bool isGoalTracked = vwoInstance.Track(campaignKey, "Ashley", "CUSTOM");
            Assert.True(isGoalTracked);
            UserStorageService.getStorage().TryGetValue(campaignKey, out ConcurrentDictionary<string, Dictionary<string, dynamic>> userMap);
            Assert.Equal("CUSTOM", userMap["Ashley"]["GoalIdentifier"]);
            Assert.Equal(variationName, userMap["Ashley"]["VariationName"]);
            ////now since one of the campaign is already present in the storage. Calling for other campaigns would return variation as null.
            variation = vwoInstance.Activate(otherCampaignKey, "Ashley");
            Assert.Null(variation);
            variationName = vwoInstance.GetVariationName(otherCampaignKey, "Ashley");
            Assert.Null(variationName);
            isGoalTracked = vwoInstance.Track(otherCampaignKey, "Ashley", "CUSTOM");
            Assert.False(isGoalTracked);
        }
        [Fact]
        public void storageFailedForCalledCampaign()
        {
            UserStorageService._userStorageMap = null;
            vwoInstance = VWO.Launch(settings, true, userStorageService: new UserStorageService());
            Logger.WriteLog(LogLevel.DEBUG, "should return null as other campaign satisfies the storage");
            var Campaigns = settings.getCampaigns();
            var campaignKey = Campaigns[3].Key;
            var otherCampaignKey = Campaigns[2].Key;
            Assert.Equal("0", UserStorageService.getStorage().Count.ToString());
            string variation = vwoInstance.Activate(otherCampaignKey, "Ashley");
            bool isGoalTracked = vwoInstance.Track(otherCampaignKey, "Ashley", "CUSTOM");
            Assert.True(isGoalTracked);
            Assert.Equal("Control", variation);
            variation = vwoInstance.Activate(campaignKey, "Ashley");
            Assert.Null(variation);
            string variationName = vwoInstance.GetVariationName(campaignKey, "Ashley");
            Assert.Null(variationName);
            isGoalTracked = vwoInstance.Track(campaignKey, "Ashley", "CUSTOM");
            Assert.False(isGoalTracked);

        }
        [Fact]
        public void campaignNotPartOFGroup()
        {
            vwoInstance = VWO.Launch(settings, true);
            Logger.WriteLog(LogLevel.DEBUG, "called campaign does not become part of MEG, variation null be assigned since campaign traffic is 10%");
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[4].Key;
            string variation = vwoInstance.Activate(calledCampaign, "Ashley");
            string variationName = vwoInstance.GetVariationName(calledCampaign, "Ashley");
            bool isGoalTracked = vwoInstance.Track(calledCampaign, "Ashley", "CUSTOM");
            Assert.Null(variation);
            Assert.Null(variationName);
            Assert.False(isGoalTracked);

        }
        [Fact]
        public void preSegmentationFailed()
        {
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[0].Key;

            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
        {

            {
              "customVariables", new Dictionary<string, dynamic>()
              {
                  {
                    "browser","chrome"
                  }
              }
            }
        };
            Campaigns[0].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"chrome", "false"}
                                        }
                                    }
                                }
                            }
                        }

        });
            Campaigns[1].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"chrome", "false"}
                                        }
                                    }
                                }
                            }
                        }

        });
            vwoInstance = VWO.Launch(settings, true);
            bool isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley", Options);
            dynamic variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "Ashley", Options);
            Assert.Null(variableValue);
            Assert.False(isFeatureEnabled);
            Campaigns[0].setSegments(new Dictionary<string, dynamic>());
            Campaigns[1].setSegments(new Dictionary<string, dynamic>());
            Campaigns[0].setPercentTraffic(0);
            Campaigns[1].setPercentTraffic(0);
            vwoInstance = VWO.Launch(settings, true);
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley");
            variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "Ashley");
            Assert.Null(variableValue);
            Assert.False(isFeatureEnabled);
        }
        [Fact]
        public void preSegmentationFailedForCalledCampaign()
        {


            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[0].Key;

            string otherCampaign = Campaigns[1].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
        {

            {
              "customVariables", new Dictionary<string, dynamic>()
              {
                  {
                    "browser","chrome"
                  }
              }
            }
        };
            Campaigns[0].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"chrome", "false"}
                                        }
                                    }
                                }
                            }
                        }

        });
            Campaigns[1].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"browser", "chrome"}
                                        }
                                    }
                                }
                            }
                        }

        });
            Logger.WriteLog(LogLevel.DEBUG, "should return false as called campaign does not satisfy the pre-segmentation condition");
            vwoInstance = VWO.Launch(settings, true);
            bool isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley", Options);
            dynamic variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "Ashley", Options);
            Assert.Null(variableValue);
            Assert.False(isFeatureEnabled);
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(otherCampaign, "Ashley", Options);
            variableValue = vwoInstance.GetFeatureVariableValue(otherCampaign, "STRING_VARIABLE", "Ashley", Options);
            Assert.NotNull(variableValue);
            Assert.True(isFeatureEnabled);
            //now changing traffic of called campaign to 0 and other campaign to 100
            Campaigns[0].setPercentTraffic(0);
            Campaigns[1].setPercentTraffic(100);
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley", Options);
            Assert.False(isFeatureEnabled);
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(otherCampaign, "Ashley", Options);
            Assert.True(isFeatureEnabled);

        }
        [Fact]
        public void preSegmentationPassedForCalledCampaign()
        {
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[0].Key;

            string otherCampaign = Campaigns[1].Key;
            Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
        {

            {
              "customVariables", new Dictionary<string, dynamic>()
              {
                  {
                    "browser","chrome"
                  }
              }
            }
        };

            Campaigns[0].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"browser", "chrome"}
                                        }
                                    }
                                }
                            }
                        }

        });
            Campaigns[1].setSegments(new Dictionary<string, dynamic>()
        {
             {
                            "or",  new List<Dictionary<string, dynamic>>()
                            {
                                new Dictionary<string, dynamic>()
                                {
                                    {
                                        "custom_variable", new Dictionary<string, dynamic>()
                                        {
                                            {"chrome", "false"}
                                        }
                                    }
                                }
                            }
                        }

        });
            Logger.WriteLog(LogLevel.DEBUG, "should return true as only called campaign satisfies the pre-segmentation condition");
            vwoInstance = VWO.Launch(settings, true);
            bool isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley", Options);
            dynamic variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "Ashley", Options);
            Assert.NotNull(variableValue);
            Assert.True(isFeatureEnabled);
            //for other campaign
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(otherCampaign, "Ashley", Options);
            Assert.False(isFeatureEnabled);
        }
        [Fact]
        public void calledCampaignWinner()
        {
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[0].Key;
            string otherCampaign = Campaigns[1].Key;

            Logger.WriteLog(LogLevel.DEBUG, "should return true/variationName as called campaign is the winner campaign after traffic normalization");
            Campaigns[0].setPercentTraffic(100);
            Campaigns[1].setPercentTraffic(100);
            vwoInstance = VWO.Launch(settings, true);

            bool isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "Ashley");
            dynamic variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "Ashley");
            Assert.NotNull(variableValue);
            Assert.True(isFeatureEnabled);
            isFeatureEnabled = vwoInstance.IsFeatureEnabled(otherCampaign, "Ashley");
            variableValue = vwoInstance.GetFeatureVariableValue(otherCampaign, "STRING_VARIABLE", "Ashley");
            Assert.Null(variableValue);
            Assert.False(isFeatureEnabled);
        }
        [Fact]
        public void calledCampaignNotWinner()
        {
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[0].Key;
            string otherCampaign = Campaigns[1].Key;
            Campaigns[0].setPercentTraffic(100);
            Campaigns[1].setPercentTraffic(100);
            Logger.WriteLog(LogLevel.DEBUG, "should return null as called campaign is the not winner campaign after traffic normalization");
            vwoInstance = VWO.Launch(settings, true);

            bool isFeatureEnabled = vwoInstance.IsFeatureEnabled(calledCampaign, "lisa");
            dynamic variableValue = vwoInstance.GetFeatureVariableValue(calledCampaign, "STRING_VARIABLE", "lisa");
            Assert.Null(variableValue);
            Assert.False(isFeatureEnabled);

        }
        [Fact]
        public void equalTrafficDistribution()
        {
            Logger.WriteLog(LogLevel.DEBUG, "should return variation after equally distributing traffic among eligible campaigns");
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[2].Key;
            Campaigns[2].setPercentTraffic(80);
            Campaigns[3].setPercentTraffic(50);
            vwoInstance = VWO.Launch(settings, true);
            string variationName = vwoInstance.Activate(calledCampaign, "Ashley");
            Assert.Equal("Variation-1", variationName);
        }
        [Fact]
        public void allNewCampaignsToTheUser()
        {
            settings = new FileReaderApiCaller("SampleGroupSettingsFile").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when both the campaigns are new to the user");
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[2].Key;
            var otherCampaign = Campaigns[3].Key;
            vwoInstance = VWO.Launch(settings, true);
            string variationName = vwoInstance.Activate(calledCampaign, "Ashley");
            Assert.Equal("Control", variationName);
            variationName = vwoInstance.Activate(otherCampaign, "Ashley");
            Assert.Null(variationName);
        }
        [Fact]
        public void newCampaignAddedToTheGroup()
        {
            settings = new FileReaderApiCaller("SampleGroupSettingsFile").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when user was already a part of a campaign and new campaign is added to the group");
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[2].Key;
            var otherCampaign = Campaigns[4].Key;
            UserStorageService._userStorageMap = null;
            vwoInstance = VWO.Launch(settings, true, userStorageService: new UserStorageService());
            string variationName = vwoInstance.Activate(calledCampaign, "Ashley");
            Assert.Equal("Control", variationName);
            settings.getCampaignGroups().Add("164", 2);
            settings.getGroups()["2"].Campaigns.Add(164);
            settings.getCampaigns()[4].setPercentTraffic(100);
            variationName = vwoInstance.Activate(otherCampaign, "Ashley");
            Assert.Null(variationName);
        }
        [Fact]
        public void viewedCampaignRemovedFromTheGroup()
        {
            settings = new FileReaderApiCaller("SampleGroupSettingsFile").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when a viewed campaign is removed from the MEG group");
            var Campaigns = settings.getCampaigns();
            var calledCampaign = Campaigns[2].Key;
            var otherCampaign = Campaigns[4].Key;
            UserStorageService._userStorageMap = null;
            vwoInstance = VWO.Launch(settings, true, userStorageService: new UserStorageService());
            string variationName = vwoInstance.Activate(calledCampaign, "Ashley");
            Assert.Equal("Control", variationName);
            settings.getCampaignGroups().Remove("162");
            settings.getGroups()["2"].Campaigns.Remove(0);
            variationName = vwoInstance.Activate(calledCampaign, "Ashley");
            Assert.Equal("Control", variationName);
        }
        [Fact]
        public void EmptyObjectReturnedWhenWinnerCampaignIsNotTheCalledCampaignAfterPriority()
        {
            settingsFileNewMEG= new FileReaderApiCaller("NewMEGSettingsFile1").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when no WinnerCampaign is found after Priority");
            var Campaigns = settingsFileNewMEG.getCampaigns();
            var calledCampaign = Campaigns[2].Key;
            vwoInstance = VWO.Launch(settingsFileNewMEG, true);
            string variation = vwoInstance.Activate(calledCampaign, "George");
            Assert.Null(variation);

        }
        [Fact]
        public void EmptyObjectReturnedWhenWinnerCampaignIsNotTheCalledCampaignAfterWeightageDistribution()
        {
            // winnerCampaign is not the called campaign for most of the times
            // distributions is 80:20 for winner and calledCampaign
            // Called campaign (id - 34) has just 20% weighted distribution
            settingsFileNewMEG = new FileReaderApiCaller("NewMEGSettingsFile2").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when no WinnerCampaign is found after Weightage distribution");
            var Campaigns = settingsFileNewMEG.getCampaigns();
            var calledCampaign = Campaigns[3].Key;
            vwoInstance = VWO.Launch(settingsFileNewMEG, true);
            
            int iterations = 1000; // number of times to call the function
            double expectedRatio = 0.2; // expected ratio for campaignId - 33 (20%) (called campaign -33)
            double allowedError = 0.05; // allowed error range (5%)

            int winners = 0;
            for (int i = 0; i < iterations; i++)
            {
                string variation = vwoInstance.Activate(calledCampaign, "George");
                winners = variation == "Control" ? winners + 1 : winners;
            }

            double actualRatio = (double)winners / iterations;
            Assert.True(actualRatio > expectedRatio - allowedError && actualRatio < expectedRatio + allowedError);
        }
        [Fact]
        public void ShouldReturnVariationWhenWinnerCampaignFoundThroughPriority()
        {
            settingsFileNewMEG = new FileReaderApiCaller("NewMEGSettingsFile1").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when WinnerCampaign is found and is same as CalledCampaign after Priority");
            var Campaigns = settingsFileNewMEG.getCampaigns();
            var calledCampaign = Campaigns[4].Key;
            vwoInstance = VWO.Launch(settingsFileNewMEG, true);
            string variation = vwoInstance.Activate(calledCampaign, "George");
            Assert.Equal("Control", variation);
    
        }
        [Fact]
        public void ShouldReturnVariationWhenWinnerCampaignFoundThroughWeightage()
        {
            settingsFileNewMEG = new FileReaderApiCaller("NewMEGSettingsFile2").GetJsonContent<Settings>();
            Logger.WriteLog(LogLevel.DEBUG, "when  WinnerCampaign is found ans is same as CalledCampaign after Weightage distribution");
            var Campaigns = settingsFileNewMEG.getCampaigns();
            var calledCampaign = Campaigns[1].Key;
            vwoInstance = VWO.Launch(settingsFileNewMEG, true);
            
            int iterations = 1000; // number of times to call the function
            double expectedRatio = 0.8; // expected ratio for campaignId - 31 (80%)
            double allowedError = 0.05; // allowed error range (5%)

            int winners = 0;
            for (int i = 0; i < iterations; i++)
            {
                string variation = vwoInstance.Activate(calledCampaign, "George");
                winners = variation == "Control" ? winners + 1 : winners;
            }

            double actualRatio = (double)winners / iterations;
            Assert.True(actualRatio > expectedRatio - allowedError && actualRatio < expectedRatio + allowedError);
        }    
    }
    public class UserStorageService : IUserStorageService
    {
        public static ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>> _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>>();
        public UserStorageService()
        {
            if (_userStorageMap == null)
                _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>>();
            try
            {

                var data = _userStorageMap;
                if (data != null)
                {
                    _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>>(data);
                }
                else
                    _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>>();
            }
            catch { }
        }

        public static ConcurrentDictionary<string, ConcurrentDictionary<string, Dictionary<string, dynamic>>> getStorage()
        {
            return _userStorageMap;
        }

        public UserStorageMap Get(string userId, string CampaignKey)
        {
            Dictionary<string, dynamic> userDict = null;
            if (_userStorageMap.TryGetValue(CampaignKey, out ConcurrentDictionary<string, Dictionary<string, dynamic>> userMap))
                userMap.TryGetValue(userId, out userDict);

            if (userDict != null)
                return new UserStorageMap(userId, CampaignKey, userDict["VariationName"], userDict["GoalIdentifier"], userDict["MetaData"]);

            return null;
        }

        public void Set(UserStorageMap userStorageMap)
        {
            if (_userStorageMap.TryGetValue(userStorageMap.CampaignKey, out ConcurrentDictionary<string, Dictionary<string, dynamic>> userMap) == false)
            {
                userMap = new ConcurrentDictionary<string, Dictionary<string, dynamic>>();
                _userStorageMap[userStorageMap.CampaignKey] = userMap;
            }
            if (userMap.ContainsKey(userStorageMap.UserId) && userMap[userStorageMap.UserId] != null && userStorageMap.GoalIdentifier != null)
            {
                userMap[userStorageMap.UserId]["GoalIdentifier"] = userStorageMap.GoalIdentifier;
            }
            else
            {
                userMap[userStorageMap.UserId] = new Dictionary<string, dynamic>() {
                    { "VariationName", userStorageMap.VariationName },
                    { "GoalIdentifier", userStorageMap.GoalIdentifier },
                    { "MetaData",userStorageMap.MetaData }
                };
            }

        }

    }
}
