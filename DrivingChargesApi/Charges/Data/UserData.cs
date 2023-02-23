namespace DrivingChargesApi.Charges.Data
{
    public class UserData
    {
        public int UserDataId { get; set; }

        public DateTime Entered { get; set; }

        public DateTime Left { get; set; }

        public string Vehicle { get; set; }
    }
}
