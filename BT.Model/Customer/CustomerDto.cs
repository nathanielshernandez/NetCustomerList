using BT.Model.Address;

namespace BT.Model.Customer
{
    public class CustomerDto : ICustomerRecord
    {
        public int CustomerID { get; set; } = -1;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public Address.Address Address { get; set; } = new Address.Address();
    }
}
