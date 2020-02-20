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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace VWOSdk
{
    internal class OperandEvaluator
    {
        private static string GroupingPattern = @"^(.+?)\((.*)\)$";
        private static string WildcardPattern = @"(^\*|^)(.+?)(\*$|$)";

        internal OperandEvaluator() {}
        public bool EvaluateOperand(Dictionary<string, dynamic> operandData, Dictionary<string, dynamic> customVariables) {
            var operandKey = operandData.Keys.First();
            string operand = operandData[operandKey];
            // Retrieve corresponding custom_variable value from custom_variables
            var customVariablesValue = customVariables.ContainsKey(operandKey) ? customVariables[operandKey] : null;
            // Pre process custom_variable value
            customVariablesValue = this.ProcessCustomVariablesValue(customVariablesValue);
            // Pre process operand value
            var procecessedOperands = this.ProcessOperandValue(operand);
            var operandType = procecessedOperands[0];
            var operandValue = procecessedOperands[1];

            // Process the customVariablesValue and operandValue to make them of same type
            string[] trueTypesData = this.ConvertToTrueTypes(operandValue, customVariablesValue);
            operandValue = trueTypesData[0];
            customVariablesValue = trueTypesData[1];
            switch (operandType) {
                case Constants.OperandValueTypes.CONTAINS:
                    return this.Contains(operandValue, customVariablesValue);
                case Constants.OperandValueTypes.STARTS_WITH:
                    return this.StartsWith(operandValue, customVariablesValue);
                case Constants.OperandValueTypes.ENDS_WITH:
                    return this.EndsWith(operandValue, customVariablesValue);
                case Constants.OperandValueTypes.LOWER:
                    return this.Lower(operandValue, customVariablesValue);
                case Constants.OperandValueTypes.REGEX:
                    return this.Regexp(operandValue, customVariablesValue);
                default:
                    // Default is case of equals to
                    return this.Equals(operandValue, customVariablesValue);
            }
        }

        private string ProcessCustomVariablesValue(dynamic customVariableValue) {
            if (customVariableValue == null || customVariableValue.ToString().Length == 0) return "";
            if (customVariableValue.GetType() == typeof(bool)) {
                customVariableValue = customVariableValue ? Constants.OperandValueBooleanTypes.TRUE : Constants.OperandValueBooleanTypes.FALSE;
            }
            return customVariableValue.ToString();
        }

        private string[] ProcessOperandValue(string operand) {
            var seperatedOperand = this.SeperateOperand(operand);
            var operandTypeName = seperatedOperand[0];
            var operandValue = seperatedOperand[1];
            if (operandTypeName == null || operandValue == null)
            {
                return new string[] { Constants.OperandValueTypes.EQUALS, operand };
            }
            var operandType = typeof(Constants.OperandValueTypesName).GetField(operandTypeName.ToUpper(), BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ToString();
            string startingStar = "";
            string endingStar = "";
            if (operandTypeName == Constants.OperandValueTypesName.WILDCARD) {
                Match match = Regex.Match(operandValue, OperandEvaluator.WildcardPattern);
                if (match.Success) {
                    startingStar = match.Groups[1].Value;
                    operandValue = match.Groups[2].Value;
                    endingStar = match.Groups[3].Value;
                }
                if (startingStar.Length > 0 && endingStar.Length > 0) {
                    operandType = Constants.OperandValueTypes.CONTAINS;
                } else if (startingStar.Length > 0) {
                    operandType = Constants.OperandValueTypes.ENDS_WITH;
                } else if (endingStar.Length > 0) {
                    operandType = Constants.OperandValueTypes.STARTS_WITH;
                } else {
                    operandType = Constants.OperandValueTypes.EQUALS;
                }
            }

            // In case there is an abnormal patter, it would have passed all the above if cases, which means it
            // Should be equals, so set the whole operand as operand value and operand type as equals
            if (operandType.Length == 0) {
                return new string[] { Constants.OperandValueTypes.EQUALS, operand };
            } else {
                return new string[] { operandType, operandValue };
            }
        }

        private string[] SeperateOperand(string operand) {
            Match match = Regex.Match(operand, OperandEvaluator.GroupingPattern);
            if (match.Success) {
                return new string[] { match.Groups[1].Value, match.Groups[2].Value };
            }
            return new string[] { Constants.OperandValueTypesName.EQUALS, operand };
        }

        private string[] ConvertToTrueTypes(dynamic operatorValue, dynamic customVariableValue) {
            try {
                var trueTypeOperatorValue = Convert.ToDouble(Convert.ToString(operatorValue));
                var trueTypeCustomVariablesValue = Convert.ToDouble(Convert.ToString(customVariableValue));
                if (trueTypeOperatorValue == Math.Floor(trueTypeOperatorValue)) trueTypeOperatorValue = Convert.ToInt32(trueTypeOperatorValue);
                if (trueTypeOperatorValue == Math.Floor(trueTypeCustomVariablesValue)) trueTypeCustomVariablesValue = Convert.ToInt32(trueTypeCustomVariablesValue);
                return new string[] { Convert.ToString(trueTypeOperatorValue), Convert.ToString(trueTypeCustomVariablesValue) };
            } catch {
                return new string[] { operatorValue, customVariableValue };
            }
        }

        private bool Contains(string operandValue, string customVariablesValue) {
            return customVariablesValue.Contains(operandValue);
        }
        private bool StartsWith(string operandValue, string customVariablesValue) {
            return customVariablesValue.StartsWith(operandValue);
        }
        private bool EndsWith(string operandValue, string customVariablesValue) {
            return customVariablesValue.EndsWith(operandValue);
        }
        private bool Lower(string operandValue, string customVariablesValue) {
            return customVariablesValue.ToLower() == operandValue.ToLower();
        }
        private bool Regexp(string operandValue, string customVariablesValue) {
            try {
                Match match = Regex.Match(customVariablesValue, operandValue);
                return match.Success;
            } catch {
                return false;
            }
        }
        private bool Equals(string operandValue, string customVariablesValue) {
            return customVariablesValue == operandValue;
        }
    }
}