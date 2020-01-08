using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    class ComboItem<T>
    {
        public T Item { get; set; } = default(T);

        public ComboItem(T item)
        {
            Item = item;
        }

        public override string ToString()
        {
            /*string str = string.Empty;
            for(int i = 1; i < Item.GetType().GetProperties().Length; i++)
            {
                str += $"{Item.GetType().GetProperties()[i].GetValue(this.Item)} ";
            }
            return str;*/

            return JsonConvert.SerializeObject(this.Item);
        }
    }
}
