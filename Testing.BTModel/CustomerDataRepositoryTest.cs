using BT.Model;
using BT.Model.Customer;
using System.Data;
using System.Data.SqlClient;

namespace Testing.BTModel
{
    [TestClass]
    public class CustomerDataRepositoryTest
    {
        private ICustomerDataRepository? _customerDataRepository;
        private IDbConnection? _dbConnection;

        [TestInitialize]
        public void Initialize()
        {
            _dbConnection = new SqlConnection("Server = localhost; Database = CustomerDb; Trusted_Connection = True;");
            _customerDataRepository = new CustomerDataRepository(_dbConnection);
        }

        [TestMethod]
        public void TestCreate()
        {
            ICustomerRecord customerRecord = new CustomerDto();
            ICustomerRecord createdCustomer = _customerDataRepository.CreateCustomer(customerRecord);
            Assert.IsNotNull(createdCustomer);
        }

        [TestMethod]
        public void TestReadAll()
        {
            IEnumerable<ICustomerRecord> customers = _customerDataRepository.GetAllCustomers();
            Assert.IsNotNull(customers);
        }

        [TestMethod]
        public void TestRead()
        {
            ICustomerRecord customerRecord = new CustomerDto();
            ICustomerRecord createdCustomer = _customerDataRepository.CreateCustomer(customerRecord);
            ICustomerRecord readCustomer = _customerDataRepository.GetCustomerById(createdCustomer.CustomerID);
            Assert.AreEqual(createdCustomer.CustomerID, readCustomer.CustomerID);
        }

        [TestMethod]
        public void TestUpdate()
        {
            ICustomerRecord customerRecord = new CustomerDto();
            ICustomerRecord createdCustomer = _customerDataRepository.CreateCustomer(customerRecord);
            createdCustomer.FirstName = "Nate";
            ICustomerRecord updatedCustomer = _customerDataRepository.UpdateCustomer(createdCustomer);
            Assert.AreEqual(createdCustomer.FirstName, updatedCustomer.FirstName);
        }

        [TestMethod]
        public void TestDelete()
        {
            ICustomerRecord customerRecord = new CustomerDto();
            ICustomerRecord createdCustomer = _customerDataRepository.CreateCustomer(customerRecord);
            bool customerDeleted = _customerDataRepository.DeleteCustomer(createdCustomer.CustomerID);
            Assert.IsTrue(customerDeleted);
        }

        [TestCleanup]
        public void Cleanup() 
        {
            _dbConnection.Dispose();
        }
    }
}