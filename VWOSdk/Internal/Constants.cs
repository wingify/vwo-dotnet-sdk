namespace VWOSdk
{
    internal class Constants
    {
        internal static readonly string PLATFORM = "server";

        internal static class Campaign
        {
            internal static readonly string STATUS_RUNNING = "RUNNING";
            internal static readonly double MAX_TRAFFIC_PERCENT = 100;
        }

        internal static class Variation
        {
            internal static readonly double MAX_TRAFFIC_VALUE = 10000;
        }

        public static class Endpoints
        {
            internal static readonly string BASE_URL = "https://dev.visualwebsiteoptimizer.com";
            internal static readonly string SERVER_SIDE = "server-side";
            internal static readonly string ACCOUNT_SETTINGS = "settings";
            internal static readonly string TRACK_USER = "track-user";
            internal static readonly string TRACK_GOAL = "track-goal";
        }
    }
}