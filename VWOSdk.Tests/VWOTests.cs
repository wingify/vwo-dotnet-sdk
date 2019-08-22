using Moq;
using Xunit;

namespace VWOSdk.Tests
{
    public class VWOTests
    {
        private readonly long MockAccountId = 123456;
        private readonly string MockSdkKey = "MockSdkKey";

        static VWOTests()
        {
            VWO.Configure(new DefaultLogWriter());
            AppContext.Configure(new FileReaderApiCaller());
        }

        public VWOTests()
        {
            
        }

        [Fact]
        public void GetSettings_Should_Return_Null_When_Validation_Fails()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            Mock.SetupGetSettings(mockValidator, false);
            VWO.Configure(mockValidator.Object);

            var result = VWO.GetSettings(MockAccountId, MockSdkKey);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetSettings(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetSettings(It.Is<long>(val => MockAccountId == val), It.Is<string>(val => MockSdkKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.Execute<Settings>(It.IsAny<ApiRequest>()), Times.Never);
            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetSettings_Should_Return_Settings_When_Validation_Is_Success()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            VWO.Configure(mockValidator.Object);

            var result = VWO.GetSettings(MockAccountId, MockSdkKey);
            Assert.NotNull(result);

            mockValidator.Verify(mock => mock.GetSettings(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetSettings(It.Is<long>(val => MockAccountId == val), It.Is<string>(val => MockSdkKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void GetSettings_Should_Return_Null_When_Api_Caller_Returns_Null()
        {
            var mockApiCaller = Mock.GetApiCaller<Settings>();
            Mock.SetupExecute<Settings>(mockApiCaller, returnValue: null);
            AppContext.Configure(mockApiCaller.Object);
            var mockValidator = Mock.GetValidator();
            VWO.Configure(mockValidator.Object);

            var result = VWO.GetSettings(MockAccountId, MockSdkKey);
            Assert.Null(result);

            mockValidator.Verify(mock => mock.GetSettings(It.IsAny<long>(), It.IsAny<string>()), Times.Once);
            mockValidator.Verify(mock => mock.GetSettings(It.Is<long>(val => MockAccountId == val), It.Is<string>(val => MockSdkKey.Equals(val))), Times.Once);

            mockApiCaller.Verify(mock => mock.ExecuteAsync(It.IsAny<ApiRequest>()), Times.Never);
        }

        [Fact]
        public void Instantiate_Should_Return_VWOClient_For_Valid_Settings_File_And_Call_Settings_Processor()
        {
            var validSettings = new FileReaderApiCaller().Execute<Settings>(null);
            var mockValidator = Mock.GetValidator();
            VWO.Configure(mockValidator.Object);
            var mockSettingProcessor = Mock.GetSettingsProcessor();
            VWO.Configure(mockSettingProcessor.Object);
            
            var vwoClient = VWO.Instantiate(validSettings);
            Assert.NotNull(vwoClient);
            Assert.IsType<VWO>(vwoClient);

            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.IsAny<Settings>()), Times.Once);
            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.Is<Settings>(val => ReferenceEquals(val, validSettings))), Times.Once);
        }

        [Fact]
        public void Instantiate_Should_Return_Null_For_InValid_Settings_File_And_Call_Settings_Processor()
        {
            var inValidSettings = new Settings(null, null, -2, -1);
            var mockValidator = Mock.GetValidator();
            Mock.SetupSettingsFile(mockValidator, false);
            VWO.Configure(mockValidator.Object);
            var mockSettingProcessor = Mock.GetSettingsProcessor();
            VWO.Configure(mockSettingProcessor.Object);

            var vwoClient = VWO.Instantiate(inValidSettings);
            Assert.Null(vwoClient);

            mockValidator.Verify(mock => mock.SettingsFile(It.IsAny<Settings>()), Times.Once);
            mockValidator.Verify(mock => mock.SettingsFile(It.Is<Settings>(val => ReferenceEquals(inValidSettings, val))), Times.Once);

            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.IsAny<Settings>()), Times.Never);
            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.Is<Settings>(val => ReferenceEquals(val, inValidSettings))), Times.Never);
        }

        [Fact]
        public void Instantiate_Should_Return_Null_When_Settings_Processor_Returns_Null_Account_Settings()
        {
            var inValidSettings = new Settings(null, null, -2, -1);
            var mockValidator = Mock.GetValidator();
            VWO.Configure(mockValidator.Object);
            var mockSettingProcessor = Mock.GetSettingsProcessor();
            //Mock.SetupProcessAndBucket(mockSettingProcessor, returnValue: null);
            VWO.Configure(mockSettingProcessor.Object);

            var vwoClient = VWO.Instantiate(inValidSettings);
            Assert.Null(vwoClient);

            mockValidator.Verify(mock => mock.SettingsFile(It.IsAny<Settings>()), Times.Once);
            mockValidator.Verify(mock => mock.SettingsFile(It.Is<Settings>(val => ReferenceEquals(inValidSettings, val))), Times.Once);

            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.IsAny<Settings>()), Times.Once);
            mockSettingProcessor.Verify(mock => mock.ProcessAndBucket(It.Is<Settings>(val => ReferenceEquals(val, inValidSettings))), Times.Once);
        }

        private bool Verify(ApiRequest apiRequest)
        {
            if (apiRequest != null)
            {
                var url = apiRequest.Uri.ToString().ToLower();
                if (url.Contains(MockAccountId.ToString().ToLower()) && url.Contains(MockSdkKey.ToString().ToLower()))
                    return true;
            }
            return false;
        }
    }
}
