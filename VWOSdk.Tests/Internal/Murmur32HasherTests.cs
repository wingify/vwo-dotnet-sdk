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

using Xunit;

namespace VWOSdk.Tests
{
    public class Murmur32HasherTests
    {
        private static readonly Murmur32BucketService _hasher = new Murmur32BucketService();

        [Theory]
        #region InlineData
        [InlineData("Ashley", 100, 1, 50, 2141232222, 0.49854447646066546)]
        [InlineData("Bill", 100, 1, 24, 1021354666, 0.23780266428366303)]
        [InlineData("Chris", 100, 1, 93, 3987558841, 0.9284258915577084)]
        [InlineData("Dominic", 100, 1, 76, 3222948109, 0.750401082681492)]
        [InlineData("Emma", 100, 1, 44, 1887162339, 0.4393892220687121)]
        [InlineData("Faizan", 100, 1, 43, 1811472472, 0.42176630161702633)]
        [InlineData("Gimmy", 100, 1, 96, 4111073909, 0.9571839843410999)]
        [InlineData("Harry", 100, 1, 26, 1078116974, 0.2510186689905822)]
        [InlineData("Ian", 100, 1, 48, 2024899030, 0.47145854448899627)]
        [InlineData("John", 100, 1, 3, 92429675, 0.021520460722967982)]
        [InlineData("King", 100, 1, 82, 3520821391, 0.8197551106568426)]
        [InlineData("Lisa", 100, 1, 4, 168829073, 0.03930858173407614)]
        [InlineData("Mona", 100, 1, 26, 1115911145, 0.25981831015087664)]
        [InlineData("Nina", 100, 1, 70, 2976878677, 0.6931085784453899)]
        [InlineData("Olivia", 100, 1, 33, 1375380229, 0.3202306639868766)]
        [InlineData("Pete", 100, 1, 65, 2772854445, 0.6456054851878434)]
        [InlineData("Queen", 100, 1, 54, 2310168113, 0.5378779286984354)]
        [InlineData("Robert", 100, 1, 27, 1150261924, 0.26781622413545847)]
        [InlineData("Sarah", 100, 1, 2, 69650962, 0.016216878313571215)]
        [InlineData("Tierra", 100, 1, 71, 3015628327, 0.702130684396252)]
        [InlineData("Una", 100, 1, 16, 663501022, 0.1544833700172603)]
        [InlineData("Varun", 100, 1, 48, 2025462540, 0.47158974688500166)]
        [InlineData("Will", 100, 1, 98, 4179029310, 0.9730060840956867)]
        [InlineData("Xin", 100, 1, 44, 1864705226, 0.43416051799431443)]
        [InlineData("You", 100, 1, 54, 2284924846, 0.5320005225948989)]
        [InlineData("Zeba", 100, 1, 38, 1601720116, 0.3729295255616307)]

        [InlineData("Ashley", 10000, 1, 4986, 2141232222, 0.49854447646066546)]
        [InlineData("Bill", 10000, 1, 2379, 1021354666, 0.23780266428366303)]
        [InlineData("Chris", 10000, 1, 9285, 3987558841, 0.9284258915577084)]
        [InlineData("Dominic", 10000, 1, 7505, 3222948109, 0.750401082681492)]
        [InlineData("Emma", 10000, 1, 4394, 1887162339, 0.4393892220687121)]
        [InlineData("Faizan", 10000, 1, 4218, 1811472472, 0.42176630161702633)]
        [InlineData("Gimmy", 10000, 1, 9572, 4111073909, 0.9571839843410999)]
        [InlineData("Harry", 10000, 1, 2511, 1078116974, 0.2510186689905822)]
        [InlineData("Ian", 10000, 1, 4715, 2024899030, 0.47145854448899627)]
        [InlineData("John", 10000, 1, 216, 92429675, 0.021520460722967982)]
        [InlineData("King", 10000, 1, 8198, 3520821391, 0.8197551106568426)]
        [InlineData("Lisa", 10000, 1, 394, 168829073, 0.03930858173407614)]
        [InlineData("Mona", 10000, 1, 2599, 1115911145, 0.25981831015087664)]
        [InlineData("Nina", 10000, 1, 6932, 2976878677, 0.6931085784453899)]
        [InlineData("Olivia", 10000, 1, 3203, 1375380229, 0.3202306639868766)]
        [InlineData("Pete", 10000, 1, 6457, 2772854445, 0.6456054851878434)]
        [InlineData("Queen", 10000, 1, 5379, 2310168113, 0.5378779286984354)]
        [InlineData("Robert", 10000, 1, 2679, 1150261924, 0.26781622413545847)]
        [InlineData("Sarah", 10000, 1, 163, 69650962, 0.016216878313571215)]
        [InlineData("Tierra", 10000, 1, 7022, 3015628327, 0.702130684396252)]
        [InlineData("Una", 10000, 1, 1545, 663501022, 0.1544833700172603)]
        [InlineData("Varun", 10000, 1, 4716, 2025462540, 0.47158974688500166)]
        [InlineData("Will", 10000, 1, 9731, 4179029310, 0.9730060840956867)]
        [InlineData("Xin", 10000, 1, 4342, 1864705226, 0.43416051799431443)]
        [InlineData("You", 10000, 1, 5321, 2284924846, 0.5320005225948989)]
        [InlineData("Zeba", 10000, 1, 3730, 1601720116, 0.3729295255616307)]
        #endregion InlineData

        public void MurmurHash32_Compute_Should_Return_Same_Hash_For_A_Particular_User(string userId, double maxVal, double multiplier, double expectedHash, long murmur, double ratio)
        {
            double hash = _hasher.ComputeBucketValue(userId, maxVal, multiplier);
            Assert.True(hash >= 0);
            Assert.True(hash <= maxVal);
            Assert.Equal(expectedHash.ToString(), hash.ToString());//Test data was generated by converting computed hash into string, hence asserting on string
        }
    }
}
