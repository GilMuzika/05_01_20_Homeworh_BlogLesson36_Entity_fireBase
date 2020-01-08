using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    interface IDAOProvider
    {
        List<Customer> GetAllCustomers();
        List<Order> GetAllOrders();
        List<Order> GetAllOrdersByCustomerId(int customerID);
        bool GetOrderById(int orderID, out Order order);
        bool GetCustomerById(int customerID, out Customer customer);
        bool AddCustomer(Customer customer);
        bool RemoveCustomer(int customerID);
        bool UpdateCustomer(Customer customer);
        bool AddOrder(Order order);
        bool RemoveOrder(int orderID);
        bool UpdateOrder(Order order);
        List<OrderCustomer> GetAllorderCustomer();

    }
}
