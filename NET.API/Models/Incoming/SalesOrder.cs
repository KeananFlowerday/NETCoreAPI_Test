namespace NET.API.Models.Incoming
{

    public class Root
    {
        public SalesOrderRequest SalesOrderRequest { get; set; }
    }

    public class SalesOrderRequest
    {
        public Salesorder SalesOrder { get; set; }
    }

    public class Salesorder
    {
        public string salesOrderRef { get; set; }
        public string orderDate { get; set; }
        public string currency { get; set; }
        public string shipDate { get; set; }
        public string categoryCode { get; set; }
        public Address[] addresses { get; set; }
        public Orderline[] orderLines { get; set; }
    }

    public class Address
    {
        public string addressType { get; set; }
        public int locationNumber { get; set; }
        public string contactName { get; set; }
        public string lastName { get; set; }
        public string companyName { get; set; }
        public string addressLine1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public int postcode { get; set; }
        public string countryCode { get; set; }
        public string phoneNumber { get; set; }
        public string emailAddress { get; set; }
        public string locationNumberQualifier { get; set; }
    }

    public class Orderline
    {
        public string skuCode { get; set; }
        public int quantity { get; set; }
        public string description { get; set; }
    }


}
