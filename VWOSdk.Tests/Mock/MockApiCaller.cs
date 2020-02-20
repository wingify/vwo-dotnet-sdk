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

namespace VWOSdk.Tests
{
    internal class MockApiCaller
    {
        internal static Mock<IApiCaller> Get<T>(IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            var mockApiCaller = new Mock<IApiCaller>();
            SetupExecute<T>(mockApiCaller, innerApiCaller);
            SetupExecuteAsync(mockApiCaller, innerApiCaller);
            return mockApiCaller;
        }

        internal static void SetupExecute<T>(Mock<IApiCaller> mockApiCaller, T returnValue = default(T))
        {
            mockApiCaller.Setup(mock => mock.Execute<T>(It.IsAny<ApiRequest>()))
                .Returns(returnValue);
        }

        internal static void SetupExecute<T>(Mock<IApiCaller> mockApiCaller, IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            mockApiCaller.Setup(mock => mock.Execute<T>(It.IsAny<ApiRequest>()))
                .Returns<ApiRequest>((rq) => innerApiCaller.Execute<T>(rq));
        }

        internal static void SetupExecuteAsync(Mock<IApiCaller> mockApiCaller, IApiCaller innerApiCaller = null)
        {
            innerApiCaller = innerApiCaller ?? new FileReaderApiCaller();
            mockApiCaller.Setup(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()))
                .Returns<ApiRequest>((rq) => innerApiCaller.ExecuteAsync(rq));
        }
    }
}
