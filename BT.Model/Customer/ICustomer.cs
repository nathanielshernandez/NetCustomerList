using BT.Model.Address;
using System.Data;

namespace BT.Model.Customer
{
    public interface ICustomerRecord
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public Address.Address Address { get; set; }
    }

    public interface ICustomer : ICustomerRecord
    {
        void Save(IDbConnection dbConnection);
        void Delete(IDbConnection dbConnection);
        ICustomerRecord GetRecord();
    }
}
