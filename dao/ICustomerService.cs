using Car_Connect.entity_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    public interface ICustomerService
    {
        Customer GetCustomerById(int customerId);
        Customer GetCustomerByUsername(string username);
        void RegisterCustomer(Customer customerData);
        void UpdateCustomer(Customer customerData);
        void DeleteCustomer(int customerId);
    }
}
