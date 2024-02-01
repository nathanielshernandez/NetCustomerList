using BT.Model.Address;
using BT.Model.Customer;
using Dapper;
using System.Data;

namespace BT.Model
{
    public class CustomerDataRepository : ICustomerDataRepository
    {
        public readonly IDbConnection _dbConnection;

        public CustomerDataRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        
        /// <summary>
        /// Creates a customer in the database
        /// </summary>
        /// <param name="customerRecord">The customer to be created</param>
        /// <returns>The created customer</returns>
        public ICustomerRecord CreateCustomer(ICustomerRecord customerRecord)
        {
            ICustomer createdCustomer = GetNewCustomer(customerRecord);
            return createdCustomer.GetRecord();
        }

        /// <summary>
        /// Gets a customer from the database by ID
        /// </summary>
        /// <param name="id">The customer's ID</param>
        /// <returns>The customer with the given ID</returns>
        /// <exception cref="NotFoundException">Could not find a customer with the given ID in the database</exception>
        public ICustomerRecord GetCustomerById(int id)
        {
            string query = @"
                SELECT Customers.CustomerID, Customers.FirstName, Customers.LastName, Customers.CompanyName, 
                   Addresses.AddressID, Addresses.Street, Addresses.City, Addresses.State, Addresses.Zip
                FROM Customers
                INNER JOIN Addresses ON Customers.AddressID = Addresses.AddressID
                WHERE Customers.CustomerID = @CustomerID;";

            var customer = _dbConnection.Query<CustomerDto, Address.Address, CustomerDto>(
                query,
                (customer, address) =>
                {
                    customer.Address = (Address.Address)address;
                    return customer;
                },
                new { CustomerID = id },
                splitOn: "AddressID").FirstOrDefault();

            return (CustomerDto)(customer ?? throw new NotFoundException($"Customer with ID {id} not found."));
        }

        /// <summary>
        /// Gets a list of all customers in the database
        /// </summary>
        /// <returns>A list of all customers</returns>
        public IEnumerable<ICustomerRecord> GetAllCustomers()
        {
            const string query = @"
                SELECT Customers.CustomerID, Customers.FirstName, Customers.LastName, Customers.CompanyName, 
                       Addresses.AddressID, Addresses.Street, Addresses.City, Addresses.State, Addresses.Zip
                FROM Customers
                INNER JOIN Addresses ON Customers.AddressID = Addresses.AddressID;";

            var customers = _dbConnection.Query<CustomerDto, Address.Address, CustomerDto>(
                query,
                (customer, address) =>
                {
                    customer.Address = address;
                    return customer;
                },
                splitOn: "AddressID"
                );

            return customers;
        }

        /// <summary>
        /// Updates a customer record in the database
        /// </summary>
        /// <param name="customerRecord">The customer's updated state</param>
        /// <returns>The updated customer record</returns>
        public ICustomerRecord UpdateCustomer(ICustomerRecord customerRecord)
        {
            Customer.Customer customer = new Customer.Customer((CustomerDto)customerRecord);
            customer.Save(_dbConnection);
            return customer.GetRecord();
        }

        /// <summary>
        /// Deletes a customer from the database
        /// </summary>
        /// <param name="id">The ID of the customer to be deleted</param>
        /// <returns>True if the customer was deleted</returns>
        public bool DeleteCustomer(int id)
        {
            ICustomerRecord customerRecord = GetCustomerById(id);
            Customer.Customer customer = new Customer.Customer((CustomerDto)customerRecord);
            customer.Delete(_dbConnection);
            return true;
        }

        /// <summary>
        /// Helper method - used to create a new customer in the database and return a Customer object
        /// </summary>
        /// <param name="customerParams">The customer to be created</param>
        /// <returns>A customer object that encapsulates the created customer in the database</returns>
        private ICustomer GetNewCustomer(ICustomerRecord? customerParams = null)
        {
            if (customerParams == null) { customerParams = new CustomerDto(); }

            IAddress adress = CreateNewAddress(customerParams.Address);

            CustomerDto customerRecord = _dbConnection.QuerySingle<CustomerDto>(
                @"
                INSERT INTO Customers
                VALUES(@FirstName, @LastName, @CompanyName, @AddressID);
                SELECT * FROM Customers WHERE CustomerID = SCOPE_IDENTITY();",
                new
                {
                    FirstName = customerParams.FirstName,
                    LastName = customerParams.LastName,
                    CompanyName = customerParams.CompanyName,
                    AddressID = adress.AddressID,
                });

            return new Customer.Customer(customerRecord);
        }

        /// <summary>
        /// Helper method - used to create a new address in the database and return an Address object
        /// </summary>
        /// <param name="addressParams">The customer to be created</param>
        /// <returns>An address object that encapsulates the created address in the database</returns>
        private IAddress CreateNewAddress(IAddress addressParams)
        {
            IAddress createdAddress = _dbConnection.QuerySingle<Address.Address>(
                @"
                INSERT INTO Addresses
                VALUES(@Street, @City, @State, @Zip);
                SELECT * FROM Addresses WHERE AddressID = SCOPE_IDENTITY();",
                addressParams);

            return createdAddress;
        }
    }
}
