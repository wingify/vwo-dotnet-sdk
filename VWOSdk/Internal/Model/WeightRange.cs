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
namespace VWOSdk
{
    internal class WeightRange
    {
        public WeightRange(double start, double end)
        {
            this.Start = start;
            this.To = end;
        }

        public double Start { get; set; }
        public double To { get; set; }

        public override bool Equals(object obj)
        {
            var range = obj as WeightRange;
            return range != null &&
                   Start == range.Start &&
                   To == range.To;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = -1781160927;
                hashCode = hashCode * -1521134295 + Start.GetHashCode();
                hashCode = hashCode * -1521134295 + To.GetHashCode();
                return hashCode;
            }
        }

        public bool IsInRange(double value)
        {
            return this.Start <= value && this.To >= value;
        }
    }
}
