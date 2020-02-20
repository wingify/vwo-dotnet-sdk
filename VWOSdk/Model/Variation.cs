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

using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk
{
    public class Variation
    {
        [JsonConstructor]
        internal Variation(int id, string name, Changes changes, double weight, bool IsFeatureEnabled, List<Dictionary<string, dynamic>> Variables = null)
        {
            this.Id = id;
            this.Name = name;
            this.Changes = changes;
            this.Weight = weight;
            this.IsFeatureEnabled = IsFeatureEnabled;
            if (this.Variables == null) this.Variables = new List<Dictionary<string, dynamic>>();
            this.Variables = Variables;
        }

        public int Id { get; internal set; }
        public string Name { get; internal set; }
        public Changes Changes { get; internal set; }
        public double Weight { get; internal set; }
        public bool IsFeatureEnabled { get; internal set; }
        public List<Dictionary<string, dynamic>> Variables { get; internal set; }
    }
}
