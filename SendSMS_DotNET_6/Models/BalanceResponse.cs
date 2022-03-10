namespace SendSMS_DotNET_6.Models
{
    public class BalanceResponse
    {
        public int error { get; set; }
        public string msg { get; set; }
        public Balance data { get; set; }

    }
    public class Balance 
    {
        public decimal balance { get; set; }
        public DateTime validity { get; set; }
    }
}
