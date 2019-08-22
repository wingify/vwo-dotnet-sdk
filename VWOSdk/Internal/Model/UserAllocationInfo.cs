namespace VWOSdk
{
    internal class UserAllocationInfo
    {
        public UserAllocationInfo()
        {
        }

        public UserAllocationInfo(Variation variation, BucketedCampaign campaign)
        {
            this.Variation = variation;
            this.Campaign = campaign;
        }

        public Goal Goal { get; set; }
        public Variation Variation { get; set; }
        public BucketedCampaign Campaign { get; set; }
    }
}