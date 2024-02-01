using BT.Model.Customer;

namespace BT.Model
{
    public interface ICustomerDataRepository
    {
        ICustomerRecord CreateCustomer(ICustomerRecord customerRecord);
        ICustomerRecord GetCustomerById(int id);
        IEnumerable<ICustomerRecord> GetAllCustomers();
        ICustomerRecord UpdateCustomer(ICustomerRecord customerRecord);
        bool DeleteCustomer(int id);    
    }
}
