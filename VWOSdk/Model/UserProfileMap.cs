namespace VWOSdk
{
    public class UserProfileMap
    {
        public UserProfileMap()
        {
            
        }

        public UserProfileMap(string userId, string campaignTestKey, string variationName)
        {
            this.UserId = userId;
            this.CampaignTestKey = campaignTestKey;
            this.VariationName = variationName;
        }

        public string UserId { get; set; }
        public string CampaignTestKey { get; set; }
        public string VariationName { get; set; }
    }
}