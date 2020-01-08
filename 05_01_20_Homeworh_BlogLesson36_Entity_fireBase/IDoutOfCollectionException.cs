using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_01_20_Homeworh_BlogLesson36_Entity_fireBase
{
    class IDoutOfCollectionException: Exception
    {
        public IDoutOfCollectionException(string message): base(message)
        { }
    }
}
