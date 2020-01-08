using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 
 * working with Entity framework explaination:
 * https://www.codeproject.com/Articles/739164/Entity-Framework-Tutorial-for-Beginners
 * 
 * */
namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    class DAOMSSQLProvider: IDAOProvider
    {
        
        /*public DAOMSSQLProvider()
        {
        }*/

        public bool AddCustomer(Customer customer)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Customers.Add(customer);
                    entitySettings.SaveChanges();
                    toReturn = true;
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool AddOrder(Order order)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Orders.Add(order);
                    entitySettings.SaveChanges();
                    toReturn = true;
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public List<Customer> GetAllCustomers()
        {
            List<Customer> listOfCustomers = new List<Customer>();
            using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
            {
                listOfCustomers.AddRange(entitySettings.Customers);
            }
            return listOfCustomers;
        }

        public List<OrderCustomer> GetAllorderCustomer()
        {
            List<Order> orders = GetAllOrders();
            List<string> customerNames = new List<string>();
            foreach(var s in orders)
            {
                GetCustomerById(s.customer_ID, out Customer customer);                
                customerNames.Add(customer.name);
            }
            List<OrderCustomer> orderCusrtomer = new List<OrderCustomer>();
            for (int i = 0; i < orders.Count; i++) 
                orderCusrtomer.Add(new OrderCustomer(orders[i].ID, orders[i].customer_ID, orders[i].price, orders[i].date, customerNames[i]));

            return orderCusrtomer;            
        }

        public List<Order> GetAllOrders()
        {
            List<Order> listOfOrders = new List<Order>();
            using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
            {
                listOfOrders.AddRange(entitySettings.Orders);
            }
            return listOfOrders;
        }

        public List<Order> GetAllOrdersByCustomerId(int customerID)
        {
            List<Order> listOfOrders = new List<Order>();
            using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
            {
                listOfOrders.AddRange(entitySettings.Orders.Where(x => x.customer_ID.Equals(customerID)));
            }
            return listOfOrders;
        }

        public bool GetCustomerById(int customerID, out Customer customer)
        {
            bool toReturn = false;
            Customer findCust = null;
            try
            {                
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Customers.ToList().ForEach(x => { if (x.ID == customerID) { findCust = x; toReturn = true; } });
                }
                
            }
            catch
            {
                toReturn = false;
            }
            customer = findCust;
            return toReturn;
        }

        public bool GetOrderById(int orderID, out Order order)
        {
            bool toReturn = false;
            Order findOrd = null;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Orders.ToList().ForEach(x => { if (x.ID == orderID) { findOrd = x; toReturn = true; } });
                }

            }
            catch
            {
                toReturn = false;
            }
            order = findOrd;
            return toReturn;
        }

        public bool RemoveCustomer(int customerID)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Customers.ToList().ForEach(x => { if (x.ID == customerID) { entitySettings.Customers.Remove(x); entitySettings.SaveChanges(); toReturn = true; } });
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool RemoveOrder(int orderID)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Orders.ToList().ForEach(x => { if (x.ID == orderID) { entitySettings.Orders.Remove(x); entitySettings.SaveChanges(); toReturn = true; } });
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool UpdateCustomer(Customer customer)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Customers.ToList().ForEach(x => { if (x.ID == customer.ID) {
                            entitySettings.Customers.Remove(x);
                            entitySettings.Customers.Add(customer);
                            entitySettings.SaveChanges(); toReturn = true; 
                        } });
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool UpdateOrder(Order order)
        {
            bool toReturn = false;
            try
            {
                using (EntityModelConnectionSettings entitySettings = new EntityModelConnectionSettings())
                {
                    entitySettings.Orders.ToList().ForEach(x => { if (x.ID == order.ID) {
                            entitySettings.Orders.Remove(x);
                            entitySettings.Orders.Add(order);
                            entitySettings.SaveChanges(); toReturn = true; 
                        } });
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }
    }
}
