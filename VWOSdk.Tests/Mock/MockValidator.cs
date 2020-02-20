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

using Moq;
using System.Collections.Generic;

namespace VWOSdk.Tests
{
    internal class MockValidator
    {
        internal static Mock<IValidator> Get()
        {
            var mockValidator = new Mock<IValidator>();
            SetupGetSettings(mockValidator, returnValue: true);
            SetupActivate(mockValidator, returnValue: true);
            SetupGetVariation(mockValidator, returnValue: true);
            SetupTrack(mockValidator, returnValue: true);
            SetupIsFeatureEnabled(mockValidator, returnValue: true);
            SetupGetFeatureVariableValue(mockValidator, returnValue: true);
            SetupPush(mockValidator, returnValue: true);
            SetupSettingsFile(mockValidator, returnValue: true);
            return mockValidator;
        }

        internal static void SetupGetSettings(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.GetSettings(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(returnValue);
        }

        internal static void SetupActivate(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.Activate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(returnValue);
        }

        internal static void SetupGetVariation(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.GetVariation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(returnValue);
        }

        internal static void SetupTrack(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.Track(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(returnValue);
        }

        internal static void SetupIsFeatureEnabled(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.IsFeatureEnabled(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(returnValue);
        }

        internal static void SetupGetFeatureVariableValue(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.GetFeatureVariableValue(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                .Returns(returnValue);
        }

        internal static void SetupPush(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.Push(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnValue);
        }


        internal static void SetupSettingsFile(Mock<IValidator> mockValidator, bool returnValue)
        {
            mockValidator.Setup(mock => mock.SettingsFile(It.IsAny<Settings>()))
                .Returns(returnValue);
        }
    }
}
