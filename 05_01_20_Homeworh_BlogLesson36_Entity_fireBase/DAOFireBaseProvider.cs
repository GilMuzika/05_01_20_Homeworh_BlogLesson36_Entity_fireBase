using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    class DAOFireBaseProvider: IDAOProvider
    {
        private IFirebaseClient _firebaseClient;
        private IFirebaseConfig _fireBaseConfig = new  FirebaseConfig();
        

        private string _secret;
        private string _url;

        public DAOFireBaseProvider()
        {
            _secret = ConfigurationManager.AppSettings["fireSecret"];
            _url = ConfigurationManager.AppSettings["fireURL"];

            _fireBaseConfig.AuthSecret = _secret;
            _fireBaseConfig.BasePath = _url;

            _firebaseClient = new FirebaseClient(_fireBaseConfig);

            if(_firebaseClient == null) throw new NullReferenceException("Connection to the FireBase didn't established!");
        }

        public bool AddCustomer(Customer customer)
        {
            return AddSomethingInternal<Customer>(customer);
        }
        private bool AddSomethingInternal<T>(T something)
        {
            bool toReturn = false;
            try
            {                
                FirebaseResponse response = _firebaseClient.Get($"{typeof(T).Name}s/{typeof(T).Name}sNumeration");
                var numberOfSomething = response.ResultAs<Numeration>();
                numberOfSomething.Number++;
                typeof(T).GetProperty("ID").SetValue(something, numberOfSomething.Number);
                _firebaseClient.Set($"{typeof(T).Name}s/{typeof(T).GetProperty("ID").GetValue(something)}", something);                
                _firebaseClient.Set($"{typeof(T).Name}s/{typeof(T).Name}sNumeration", new Numeration { Number = numberOfSomething.Number }); 
                toReturn = true;
            }
            catch(Exception ex)
            {
                toReturn = false;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            return toReturn;


        }

        public bool AddOrder(Order order)
        {
            return AddSomethingInternal<Order>(order);
        }

        public List<Customer> GetAllCustomers()
        {
            return GetAllSomethingInternal<Customer>();
        }
        private List<T> GetAllSomethingInternal<T>()
        {
            List<T> listOfSomething = new List<T>();
            try
            {
                FirebaseResponse response = _firebaseClient.Get($"{typeof(T).Name}s/{typeof(T).Name}sNumeration");                
                var numberOfSomething = response.ResultAs<Numeration>().Number;
                for (int i = 1; i <= numberOfSomething; i++)
                {
                    response = _firebaseClient.Get($"{typeof(T).Name}s/{i}");
                    var result = response.ResultAs<T>();
                    listOfSomething.Add(result);
                }
            }
            catch (Exception ex)
            {                
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            //MessageBox.Show(JsonConvert.SerializeObject(result));
            return listOfSomething;
        }

        public List<OrderCustomer> GetAllorderCustomer()
        {
            List<OrderCustomer> orderCustomers = new List<OrderCustomer>();
            try
            {
                List<Order> orders = GetAllOrders();
                for (int i = 0; i < orders.Count; i++)
                {
                    GetCustomerById(orders[i].customer_ID, out Customer customer);
                    if (orders[i].customer_ID.Equals(customer.ID))
                        orderCustomers.Add(new OrderCustomer(orders[i].ID, orders[i].customer_ID, orders[i].price, orders[i].date, customer.name));
                }
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            return orderCustomers;
        }

        public List<Order> GetAllOrders()
        {
            return GetAllSomethingInternal<Order>();
        }

        public List<Order> GetAllOrdersByCustomerId(int customerID)
        {
            return GetAllSomethingInternal<Order>().Where(x => x.customer_ID.Equals(customerID)).ToList();
        }

        public bool GetCustomerById(int customerID, out Customer customer)
        {
            return getSomethingById<Customer>(customerID, out customer);
        }
        private bool getSomethingById<T>(int ID, out T something)
        {
            bool toReturn = false;
            T result = default(T);
            try
            {
                FirebaseResponse response = _firebaseClient.Get($"{typeof(T).Name}s/{ID}");
                result = response.ResultAs<T>();                
                toReturn = true;                
            }
            catch (Exception ex)
            {
                toReturn = false;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            something = result;
            return toReturn;

        }

        public bool GetOrderById(int orderID, out Order order)
        {
            return getSomethingById<Order>(orderID, out order);
        }

        public bool RemoveCustomer(int customerID)
        {
            return RemoveSomethingInternal<Customer>(customerID);
        }

        public bool RemoveOrder(int orderID)
        {
            return RemoveSomethingInternal<Order>(orderID);
        }

        private bool RemoveSomethingInternal<T>(int ID)
        {            
            DeleteResponse deleteResponse = _firebaseClient.Delete($"{typeof(T).Name}s/{ID}");
            if (deleteResponse.Success)
            {
                FirebaseResponse response = _firebaseClient.Get($"{typeof(T).Name}s/{typeof(T).Name}sNumeration");
                var numberOfSomething = response.ResultAs<Numeration>();
             
                for (int i = ID + 1; i <= numberOfSomething.Number; i++)
                {
                    response = _firebaseClient.Get($"{typeof(T).Name}s/{i}");
                    var result = response.ResultAs<T>();
                    typeof(T).GetProperty("ID").SetValue(result, i - 1);
                    _firebaseClient.Set($"{typeof(T).Name}s/{i-1}", result);
                    deleteResponse = _firebaseClient.Delete($"{typeof(T).Name}s/{i}");                    
                }

                numberOfSomething.Number--;
                _firebaseClient.Set($"{typeof(T).Name}s/{typeof(T).Name}sNumeration", new Numeration { Number = numberOfSomething.Number });
            }
            return deleteResponse.Success;
        }

        public bool UpdateCustomer(Customer customer)
        {
            return UpdateSomethingInternal<Customer>(customer);
        }

        public bool UpdateOrder(Order order)
        {
            return UpdateSomethingInternal<Order>(order);
        }

        private bool UpdateSomethingInternal<T>(T something)
        {
            bool toReturn = false;
            try
            {
                FirebaseResponse response = _firebaseClient.Get($"{typeof(T).Name}s/{typeof(T).Name}sNumeration");
                var numberOfSomething = response.ResultAs<Numeration>().Number;

                if ((int)typeof(T).GetProperty("ID").GetValue(something) < 1 || (int)typeof(T).GetProperty("ID").GetValue(something) > numberOfSomething) throw new IDoutOfCollectionException($"The ID of the {typeof(T).Name.ToLower()} {JsonConvert.SerializeObject(something)} is not in the respective collection in the database");

                response = _firebaseClient.Update($"{typeof(T).Name}s/{typeof(T).GetProperty("ID").GetValue(something)}", something);
                toReturn = true;
            }
            catch (Exception ex)
            {
                toReturn = false;
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\n{ex.Message}\n\n{ex.StackTrace}");
            }
            return toReturn;
        }
    }
}
