using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    public partial class MainForm : Form
    {
        private DAOMSSQLProvider currentMSSQLProvider = new DAOMSSQLProvider();
        private DAOFireBaseProvider currentFireProvider = new DAOFireBaseProvider();

        private Random _rnd = new Random();
        private string[] namesToUsing;
        private string[] countriesToUsing;
        private List<OrderCustomer> orderCustomersMain = new List<OrderCustomer>();
        public MainForm()
        {
            InitializeComponent();
            ReadFromFile();
            Initialize();
            InitializeControlsDirectPanel();
            InitializeCombosDirectPanel();
            cmbAllTheOrdersMain_Initialize();
            InitializeControlsMain();

        }
        private void Initialize()
        {
            cmbNamesMain.Items.AddRange(namesToUsing);
            cmbCountriesMain.Items.AddRange(countriesToUsing);
            List<Customer> mssqlCustomers = currentMSSQLProvider.GetAllCustomers();
            List<Customer> fireCustomers = currentFireProvider.GetAllCustomers();
            List<Customer> allTheCustomers = new List<Customer>();
            mssqlCustomers.ForEach(x => { if (!allTheCustomers.Contains(x)) { allTheCustomers.Add(x); } });
            fireCustomers.ForEach(x => { if (!allTheCustomers.Contains(x)) { allTheCustomers.Add(x); } });
            cmbAllTheCustomersMain.Items.AddRange(allTheCustomers.Select(x => new ComboItem<Customer>(x)).ToArray());

            lblCustomerMain.drawBorder(1, Color.Black);
            lblOrderMain.drawBorder(1, Color.Black);

            List<OrderCustomer> mssqlOrderCustomers = currentMSSQLProvider.GetAllorderCustomer();
            List<OrderCustomer> fireOrderCustomers = currentFireProvider.GetAllorderCustomer();
            mssqlOrderCustomers.ForEach(x => { if (!orderCustomersMain.Contains(x)) orderCustomersMain.Add(x); } );
            fireOrderCustomers.ForEach(x => { if (!orderCustomersMain.Contains(x)) orderCustomersMain.Add(x); });




        }
        private void cmbAllTheOrdersMain_Initialize()
        {
            List<Order> mssqlOrders = currentMSSQLProvider.GetAllOrders();
            List<Order> fireorders = currentFireProvider.GetAllOrders();
            List<Order> AllTheOrders = new List<Order>();
            mssqlOrders.ForEach(x => { if (!AllTheOrders.Contains(x)) { AllTheOrders.Add(x); } });
            fireorders.ForEach(x => { if (!AllTheOrders.Contains(x)) { AllTheOrders.Add(x); } });
            cmbAllTheOrdersMain.Items.AddRange(AllTheOrders.Select(x => new ComboItem<Order>(x)).ToArray());
        }
        private void InitializeControlsMain()
        {
            cmbNamesMain.SelectedIndex = _rnd.Next(cmbNamesMain.Items.Count - 1);
            cmbCountriesMain.SelectedIndex = _rnd.Next(cmbCountriesMain.Items.Count - 1);
            cmbAllTheCustomersMain.SelectedIndex = _rnd.Next(cmbAllTheCustomersMain.Items.Count - 1);
            cmbAllTheOrdersMain.SelectedIndex = _rnd.Next(0,cmbAllTheOrdersMain.Items.Count - 1);
            numOrderPriceMain.Value = _rnd.Next((int)numOrderPriceMain.Minimum, (int)numOrderPriceMain.Maximum);
            numAgeMain.Value = _rnd.Next((int)numAgeMain.Minimum, (int)numAgeMain.Maximum);
        }
        private void InitializeCombosDirectPanel()
        {
            cmbMSSQLCustomers.Items.AddRange(currentMSSQLProvider.GetAllCustomers().Select(x => new ComboItem<Customer>(x)).ToArray());
            cmbMSSQLOrders.Items.AddRange(currentMSSQLProvider.GetAllOrders().Select(x => new ComboItem<Order>(x)).ToArray());
            cmbFireCustomers.Items.AddRange(currentFireProvider.GetAllCustomers().Select(x => new ComboItem<Customer>(x)).ToArray());
            cmbFireOrders.Items.AddRange(currentFireProvider.GetAllOrders().Select(x => new ComboItem<Order>(x)).ToArray());
        }
        private void InitializeControlsDirectPanel()
        {
            txtMSSQLName.Text = namesToUsing[_rnd.Next(namesToUsing.Length - 1)];
            txtFireName.Text = namesToUsing[_rnd.Next(namesToUsing.Length - 1)];
            txtMSSQLCountry.Text = countriesToUsing[_rnd.Next(countriesToUsing.Length - 1)];
            txtFireCountry.Text = countriesToUsing[_rnd.Next(countriesToUsing.Length - 1)];
            numMSSQLAge.Value = _rnd.Next((int)numMSSQLAge.Minimum, (int)numMSSQLAge.Maximum);
            numMSSQLPrice.Value = _rnd.Next((int)numMSSQLPrice.Minimum, (int)numMSSQLPrice.Maximum);
            numFireAge.Value = _rnd.Next((int)numFireAge.Minimum, (int)numFireAge.Maximum);
            numFirePrice.Value = _rnd.Next((int)numFirePrice.Minimum, (int)numFirePrice.Maximum);

        }

        private void ReadFromFile()
        {
            string names = string.Empty;
            string countries = string.Empty;
            try
            {
                names = File.ReadAllText("_names.txt");
                countries = File.ReadAllText("_countries.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message} \n\nSo the program will use the defult names");
                names = " Alfred Benny Connnor Daniel Eran ";
            }
            namesToUsing = names.Split(new char[] { ' ', '\t', '\n' }).Where(x => !String.IsNullOrEmpty(x)).ToArray();
            countriesToUsing = countries.Split(new char[] { ' ', '\t', '\n' }).Where(x => !String.IsNullOrEmpty(x)).ToArray();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer
            {
                ID = 0,
                Age = (int?)numAgeMain.Value,
                name = cmbNamesMain.SelectedItem as string,
                Country = cmbCountriesMain.SelectedItem as string                
            };

            currentMSSQLProvider.AddCustomer(customer);
            currentFireProvider.AddCustomer(customer);
            cmbAllTheCustomersMain.Items.Add(new ComboItem<Customer>(customer));
            InitializeControlsMain();
        }

        private void btnMSSQLNewCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer { Age = (int?)numMSSQLAge.Value, Country = txtMSSQLCountry.Text, name = txtMSSQLName.Text };
            currentMSSQLProvider.AddCustomer(customer);
            cmbMSSQLCustomers.Items.Add(new ComboItem<Customer>(customer));
            InitializeControlsDirectPanel();
        }
        

        private void btnMSSQLDeleteCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                currentMSSQLProvider.RemoveCustomer((cmbMSSQLCustomers.SelectedItem as ComboItem<Customer>).Item.ID);
                cmbMSSQLCustomers.Items.Remove(cmbMSSQLCustomers.SelectedItem as ComboItem<Customer>);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}");
            }
        }

        private void btnMSSQLUpdateCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = null;
            Customer newCustomer = null;
            try
            {
                customer = (cmbMSSQLCustomers.SelectedItem as ComboItem<Customer>).Item;
                newCustomer = new Customer
                {
                    ID = customer.ID,
                    Age = (int)numMSSQLAge.Value,
                    Country = txtMSSQLCountry.Text,
                    name = txtMSSQLName.Text
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}");
            }
                        

            currentMSSQLProvider.UpdateCustomer(newCustomer);            
            cmbMSSQLCustomers.Items.Clear();
            cmbMSSQLCustomers.Items.AddRange(currentMSSQLProvider.GetAllCustomers().Select(x => new ComboItem<Customer>(x)).ToArray());


            InitializeControlsDirectPanel();
        }
        //private void 
        private void btnMSSQLNewOrder_Click(object sender, EventArgs e)
        {
            Order order = null;
            try
            {
                order = new Order
                {
                    customer_ID = (cmbMSSQLCustomers.SelectedItem as ComboItem<Customer>).Item.ID,
                    date = DateTime.Now,
                    price = numMSSQLPrice.Value
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}");
            }
            currentMSSQLProvider.AddOrder(order);
            cmbMSSQLOrders.Items.Add(new ComboItem<Order>(order));
            
        }

        private void btnMSSQLDeleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                currentMSSQLProvider.RemoveOrder((cmbMSSQLOrders.SelectedItem as ComboItem<Order>).Item.ID);
            }
            catch(Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of orders\n\n{ex.Message}");
            }
            cmbMSSQLOrders.Items.Remove(cmbMSSQLOrders.SelectedItem as ComboItem<Order>);
        }

        private void btnMSSQLUpdateOrder_Click(object sender, EventArgs e)
        {
            Order order = null;
            Order newOrder = null;
            try
            {
                order = (cmbMSSQLOrders.SelectedItem as ComboItem<Order>).Item;


                newOrder = new Order
                {
                    ID = order.ID,
                    customer_ID = (cmbMSSQLCustomers.SelectedItem as ComboItem<Customer>).Item.ID,
                    date = DateTime.Now,
                    price = numMSSQLPrice.Value
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of orders\n\n{ex.Message}");
            }
            currentMSSQLProvider.UpdateOrder(newOrder);
            cmbMSSQLOrders.Items.Clear();
            cmbMSSQLOrders.Items.AddRange(currentMSSQLProvider.GetAllOrders().Select(x => new ComboItem<Order>(x)).ToArray());
            InitializeControlsDirectPanel();
        }

        private void btnFireNewCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = new Customer { Age = (int?)numFireAge.Value, Country = txtFireCountry.Text, name = txtFireName.Text  };
            currentFireProvider.AddCustomer(customer);
            cmbFireCustomers.Items.Add(new ComboItem<Customer>(customer));
            InitializeControlsDirectPanel();
        }

        private void btnFireNeworder_Click(object sender, EventArgs e)
        {
            Order order = null;
            try
            {
                order = new Order
                {
                    customer_ID = (cmbFireCustomers.SelectedItem as ComboItem<Customer>).Item.ID,
                    date = DateTime.Now,
                    price = numFirePrice.Value
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}");
            }
            currentFireProvider.AddOrder(order);
            cmbFireOrders.Items.Add(new ComboItem<Order>(order));
            InitializeControlsDirectPanel();
        }

        private void btnFireDeleteCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = null;
            try
            {
                customer = (cmbFireCustomers.SelectedItem as ComboItem<Customer>).Item;
                currentFireProvider.RemoveCustomer(customer.ID);
                cmbFireCustomers.Items.Remove(cmbFireCustomers.SelectedItem as ComboItem<Customer>);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}");
            }
        }

        private void btnFireupdateCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = null;
            Customer newCustomer = null;
            try
            {
                customer = (cmbFireCustomers.SelectedItem as ComboItem<Customer>).Item;
                newCustomer = new Customer
                {
                    ID = customer.ID,
                    Age = (int)numMSSQLAge.Value,
                    Country = txtMSSQLCountry.Text,
                    name = txtMSSQLName.Text
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select customer from the combo of customers\n\n{ex.Message}\n\n{ex.StackTrace}");
            }

            currentFireProvider.UpdateCustomer(newCustomer);
            cmbFireCustomers.Items.Clear();
            cmbFireCustomers.Items.AddRange(currentMSSQLProvider.GetAllCustomers().Select(x => new ComboItem<Customer>(x)).ToArray());


            InitializeControlsDirectPanel();
        }

        private void btnFireUpdateOrder_Click(object sender, EventArgs e)
        {
            Order order = null;
            Order newOrder = null;
            try
            {
                order = (cmbFireOrders.SelectedItem as ComboItem<Order>).Item;


                newOrder = new Order
                {
                    ID = order.ID,
                    customer_ID = (cmbFireCustomers.SelectedItem as ComboItem<Customer>).Item.ID,
                    date = DateTime.Now,
                    price = numFirePrice.Value
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of orders\n\n{ex.Message}");
            }
            currentMSSQLProvider.UpdateOrder(newOrder);
            cmbFireOrders.Items.Clear();
            cmbFireOrders.Items.AddRange(currentMSSQLProvider.GetAllOrders().Select(x => new ComboItem<Order>(x)).ToArray());
            InitializeControlsDirectPanel();
        }

        private void btnFireDeleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                currentFireProvider.RemoveOrder((cmbFireOrders.SelectedItem as ComboItem<Order>).Item.ID);
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of orders\n\n{ex.Message}");
            }
            cmbFireOrders.Items.Remove(cmbMSSQLOrders.SelectedItem as ComboItem<Order>);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            Order order = null;
            try
            {
                order = new Order
                {
                    customer_ID = (cmbAllTheCustomersMain.SelectedItem as ComboItem<Customer>).Item.ID,
                    date = DateTime.Now,
                    price = numOrderPriceMain.Value
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of customers\n\n{ex.Message}");
            }
            currentMSSQLProvider.AddOrder(order);
            currentFireProvider.AddOrder(order);
            cmbAllTheOrdersMain.Items.Add(new ComboItem<Order>(order));
            InitializeControlsMain();
        }

        private void btnMainUpdateCustomer_Click(object sender, EventArgs e)
        {
            Customer customer = null;
            try
            {
                customer = new Customer
                {
                    ID = (cmbAllTheCustomersMain.SelectedItem as ComboItem<Customer>).Item.ID,
                    Age = (int?)numAgeMain.Value,
                    name = cmbNamesMain.SelectedItem as string,
                    Country = cmbCountriesMain.SelectedItem as string
                };
            }
            catch (Exception ex)
            {
                FlexibleMessageBox.Show($"{ex.GetType().Name}\n\nPlease select order from the combo of orders\n\n{ex.Message}");
            }

            bool fireUpdated = false;
            bool mssqlUpdated = false;
            if (currentFireProvider.GetAllCustomers().Contains(customer)) fireUpdated = currentFireProvider.UpdateCustomer(customer);
            else currentFireProvider.AddCustomer(customer);
            if (currentMSSQLProvider.GetAllCustomers().Contains(customer)) mssqlUpdated = currentMSSQLProvider.UpdateCustomer(customer);
            else currentMSSQLProvider.AddCustomer(customer);

            if (fireUpdated && mssqlUpdated)
            {
                foreach (ComboItem<Customer> s in cmbAllTheCustomersMain.Items)
                {
                    if (s.Item.ID.Equals(customer.ID))
                    {
                        cmbAllTheCustomersMain.Items.Remove(s);
                        cmbAllTheCustomersMain.Items.Add(new ComboItem<Customer>(customer));
                    }
                }
            }
            else
            {
                cmbAllTheCustomersMain.Items.Add(new ComboItem<Customer>(customer));
                FlexibleMessageBox.Show($"One of the two databases did not contain the current customer, so it won't be updated on it");
            }



        }

        private void btnMainDeleteCustomer_Click(object sender, EventArgs e)
        {
            bool mssqlRemoved = false;
            bool fireRemoved = false;
            Customer customer = (cmbAllTheCustomersMain.SelectedItem as ComboItem<Customer>).Item;
            if (currentMSSQLProvider.GetAllCustomers().Contains(customer)) mssqlRemoved = currentMSSQLProvider.RemoveCustomer(customer.ID);
            if (currentFireProvider.GetAllCustomers().Contains(customer)) fireRemoved = currentFireProvider.RemoveCustomer(customer.ID);
            if(mssqlRemoved && fireRemoved)
            {
                foreach(ComboItem<Customer> s in cmbAllTheCustomersMain.Items)
                    if (s.Item.Equals(customer)) cmbAllTheCustomersMain.Items.Remove(s);                
            }
        }

        private void btnUpdateOrderMain_Click(object sender, EventArgs e)
        {
            Order order = (cmbAllTheOrdersMain.SelectedItem as ComboItem<Order>).Item;
            Order newOrder = new Order
            {
                ID = order.ID,
                customer_ID = (cmbAllTheCustomersMain.SelectedItem as ComboItem<Customer>).Item.ID,
                date = DateTime.Now,
                price = numOrderPriceMain.Value
            };
            bool mssqlorderUpdated = false;
            mssqlorderUpdated = currentMSSQLProvider.UpdateOrder(newOrder);
            bool fireOrderUpdated = false;
            fireOrderUpdated = currentFireProvider.UpdateOrder(newOrder);

            cmbAllTheOrdersMain_Initialize();

            InitializeControlsMain();
        }

        private void btnDeleteOrderMain_Click(object sender, EventArgs e)
        {
            Order order = (cmbAllTheOrdersMain.SelectedItem as ComboItem<Order>).Item;

            bool msssqlOrderRemoved = currentMSSQLProvider.RemoveOrder(order.ID);
            bool fireOrderRemoved = currentFireProvider.RemoveOrder(order.ID);

            if (msssqlOrderRemoved || fireOrderRemoved)
            {
                //foreach(ComboItem<Order> s in cmbAllTheOrdersMain.Items)
                for(int i = 0; i < cmbAllTheOrdersMain.Items.Count; i++)
                {
                    if ((cmbAllTheOrdersMain.Items[i] as ComboItem<Order>).Item.Equals(order)) cmbAllTheOrdersMain.Items.Remove(cmbAllTheOrdersMain.Items[i]);
                }
            }

            InitializeControlsMain();
        }

        private void cmbAllTheCustomersMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCustomerMain.Text = JsonConvert.SerializeObject(((sender as ComboBox).SelectedItem as ComboItem<Customer>).Item);
        }

        private void cmbAllTheOrdersMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblOrderMain.Text = "OrderCustomer:\n";
            Order order = ((sender as ComboBox).SelectedItem as ComboItem<Order>).Item;
            OrderCustomer ok = orderCustomersMain.Where(x => x.customer_ID == order.customer_ID).First();
            lblOrderMain.Text += Environment.NewLine;
            lblOrderMain.Text += "Name: " + ok.CustomerName + Environment.NewLine;
            lblOrderMain.Text += "Price: " + ok.price + Environment.NewLine;
            lblOrderMain.Text += "Date: " + ok.date.ToString();
        }
    }
}
