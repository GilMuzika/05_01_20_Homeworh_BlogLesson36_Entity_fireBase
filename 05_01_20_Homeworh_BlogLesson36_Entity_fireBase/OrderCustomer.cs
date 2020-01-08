using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    class OrderCustomer
    {
        public int ID { get; set; }
        public int customer_ID { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string CustomerName { get; set; }

        public OrderCustomer(int iD, int customer_ID, decimal? price, DateTime? date, string customerName)
        {
            ID = iD;
            this.customer_ID = customer_ID;
            this.price = price;
            this.date = date;
            CustomerName = customerName;
        }
    }
}
