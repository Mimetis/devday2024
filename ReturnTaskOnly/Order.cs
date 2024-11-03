namespace ReturnTaskOnly
{
    public class Order
    {
        public required string OrderId { get; set; }
        public required string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Double OrderAmount { get; set; }

    }
}
