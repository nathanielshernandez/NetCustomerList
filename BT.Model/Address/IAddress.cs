namespace BT.Model.Address
{
    public interface IAddressRecord
    {
        public int AddressID { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int Zip { get; set; }
    }

    public interface IAddress : IAddressRecord
    {
        public string AddressBlock() { throw new NotImplementedException(); }
    }
}
