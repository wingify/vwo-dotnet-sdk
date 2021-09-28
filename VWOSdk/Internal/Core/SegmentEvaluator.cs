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
using System.Linq;
using System;
using Newtonsoft.Json;
namespace VWOSdk
{
    internal class SegmentEvaluator : ISegmentEvaluator
    {
        private static readonly string file = typeof(SegmentEvaluator).FullName;

        private readonly OperandEvaluator operandEvaluator;

        internal SegmentEvaluator()
        {
            this.operandEvaluator = new OperandEvaluator();
        }

        public bool evaluate(string userId, string campaignKey, string segmentationType, Dictionary<string, dynamic> segments, Dictionary<string, dynamic> customVariables)
        {
            var result = this.evaluateSegment(segments, customVariables);
            if (segmentationType == Constants.SegmentationType.PRE_SEGMENTATION)
            {
                if (result)
                {
                    LogInfoMessage.UserPassedPreSegmentation(typeof(SegmentEvaluator).FullName, userId, campaignKey, customVariables);
                }
                else
                {
                    LogInfoMessage.UserFailedPreSegmentation(typeof(SegmentEvaluator).FullName, userId, campaignKey, customVariables);
                }
            }
            return result;
        }

        public dynamic getTypeCastedFeatureValue(dynamic value, string variableType)
        {
            try
            {
                if (value.GetType().Name == Constants.DotnetVariableTypes.VALUES[variableType])
                {
                    return value;
                }
                if (variableType == Constants.VariableTypes.STRING)
                {
                    return Convert.ToString(value);
                }
                if (variableType == Constants.VariableTypes.INTEGER)
                {
                    return Convert.ToInt32(value);
                }
                if (variableType == Constants.VariableTypes.DOUBLE)
                {
                    return Convert.ToDouble(value);
                }
                if (variableType == Constants.VariableTypes.BOOLEAN)
                {
                    return Convert.ToBoolean(value);
                }
                if (variableType == Constants.VariableTypes.JSON)
                {
                    var jsonFeatureValue = JsonConvert.SerializeObject(value);
                    if (IsValidJson(jsonFeatureValue))
                    {
                        return value;
                    }
                    else
                    {
                        LogErrorMessage.UnableToParseJson(typeof(IVWOClient).FullName, jsonFeatureValue, variableType);                       
                        return null;
                    }

                }

                return value;
            }
            catch
            {
                LogErrorMessage.UnableToTypeCast(typeof(IVWOClient).FullName, value, variableType, value.GetType().Name);
                return null;
            }
        }
        public bool IsValidJson(string input)
        {
            input = input.Trim();
            
                try
                {
                    //parse the input into a JObject
                    var jObject = JObject.Parse(input);
                    foreach (var jo in jObject)
                    {
                        string name = jo.Key;
                        JToken value = jo.Value;                      
                        if (value.Type == JTokenType.Undefined)
                        {
                            return false;
                        }
                    }
                }               
                catch 
                {                    
                    return false;
                }
           

            return true;
        }
        private bool evaluateSegment(Dictionary<string, dynamic> segments, Dictionary<string, dynamic> customVariables)
        {
            if (segments.Count == 0)
            {
                return true;
            }
            var segmentOperator = segments.Keys.First();
            var subSegments = ToDictionary(segments[segmentOperator]);
            switch (segmentOperator)
            {
                case Constants.OperatorTypes.NOT:
                    return !this.evaluateSegment(subSegments, customVariables);
                case Constants.OperatorTypes.AND:
                    foreach (var subSegment in subSegments)
                    {
                        var segment = ToDictionary(subSegment);
                        if (!this.evaluateSegment(segment, customVariables))
                        {
                            return false;
                        }
                    }
                    return true;
                case Constants.OperatorTypes.OR:
                    foreach (var subSegment in subSegments)
                    {
                        var segment = ToDictionary(subSegment);
                        if (this.evaluateSegment(segment, customVariables))
                        {
                            return true;
                        }
                    }
                    return false;
                case Constants.OperandTypes.CUSTOM_VARIABLE:
                    return this.operandEvaluator.EvaluateOperand(subSegments, customVariables);
                case Constants.OperandTypes.USER:
                    return this.operandEvaluator.EvaluateUser(subSegments, customVariables);
                default:
                    return true;
            }
        }

        private static dynamic ToDictionary(dynamic input)
        {
            if (input.GetType() == typeof(JObject))
            {
                return JObject.FromObject(input).ToObject<Dictionary<string, dynamic>>();
            }
            return input;
        }
    }
}
