namespace MainTainSenseAPI.Models
{
    public enum ActiveStatus // use to show whether active or inactive
    {
        Inactive = 0,
        Active =1,
    }
    public enum AssetStatus //used in assets model
    {
        Active,
        Retired,
        InRepair,
        InUpgrade
        // Add other possible statuses as needed
    }
    public enum YesNo
    {   
        Yes,
        No
    }
}
