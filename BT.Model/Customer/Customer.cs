using BT.Model.Address;
using Dapper;
using System.Data;

namespace BT.Model.Customer
{
    public class Customer : ICustomer
    {
        private CustomerDto _customerRecord;

        public Customer(CustomerDto? customerRecord = null)
        {
            if (customerRecord == null) { customerRecord = new CustomerDto(); }
            _customerRecord = customerRecord;
        }

        public int CustomerID
        {
            get { return _customerRecord.CustomerID; }
            set { _customerRecord.CustomerID = value; }
        }

        public string FirstName
        {
            get { return _customerRecord.FirstName; }
            set { _customerRecord.FirstName = value; }
        }

        public string LastName
        {
            get { return _customerRecord.LastName; }
            set { _customerRecord.LastName = value; }
        }

        public string CompanyName
        {
            get { return _customerRecord.CompanyName; }
            set { _customerRecord.CompanyName = value; }
        }

        public Address.Address Address
        {
            get { return (Address.Address)_customerRecord.Address; }
            set { _customerRecord.Address = value; }
        }

        public void Save(IDbConnection dbConnection)
        {
            if (_customerRecord.Address.AddressID != -1) { UpdateAddress(dbConnection); }

            string query = @"
                UPDATE Customers
                SET FirstName = @FirstName, LastName = @LastName, CompanyName = @CompanyName
                WHERE CustomerID = @CustomerID";

            int rowsAffected = dbConnection.Execute(query,
            new
            {
                FirstName = _customerRecord.FirstName,
                LastName = _customerRecord.LastName,
                CompanyName = _customerRecord.CompanyName,
                CustomerID = _customerRecord.CustomerID,
            });

            if (rowsAffected < 1)
            {
                throw new InvalidOperationException("Customer update operation was not successful");
            }
        }

        public void Delete(IDbConnection dbConnection)
        {
            string query = @"
                DELETE FROM Customers WHERE CustomerID = @CustomerID;
                DELETE FROM Addresses WHERE AddressID = @AddressID;";

            int rowsAffected = dbConnection.Execute(query, new { CustomerID = _customerRecord.CustomerID, AddressID = _customerRecord.Address.AddressID });

            if (rowsAffected < 2) 
            { 
                throw new InvalidOperationException("Customer deletion operation was not successful"); 
            }
        }

        public ICustomerRecord GetRecord()
        {
            return _customerRecord;
        }

        private void UpdateAddress(IDbConnection dbConnection)
        {
            string query = @"
                UPDATE Addresses
                SET Street = @Street, City = @City, State = @State, Zip = @Zip
                WHERE AddressID = @AddressID;";

            int rowsAffected = dbConnection.Execute(query,
            new
            {
                Street = _customerRecord.Address.Street,
                City = _customerRecord.Address.City,
                State = _customerRecord.Address.State,
                Zip = _customerRecord.Address.Zip,
                AddressID = _customerRecord.Address.AddressID
            });

            if (rowsAffected < 1)
            {
                throw new InvalidOperationException("Address update operation was not successful");
            }

        }
    }
}
