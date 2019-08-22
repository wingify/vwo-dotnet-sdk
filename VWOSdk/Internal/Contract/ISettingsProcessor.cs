namespace VWOSdk
{
    internal interface ISettingsProcessor
    {
        AccountSettings ProcessAndBucket(Settings settings);
    }
}