namespace CustomerConsumer.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string AddressType { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int Number { get; set; }
        public string City { get; set; } = string.Empty;
        public int ZipCode { get; set; }
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
