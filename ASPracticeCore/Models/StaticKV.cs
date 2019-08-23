using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPracticeCore.Models
{
    public class StaticKV:EntityBase
    {
        
        public StaticKV()
        {

        }
        public StaticKV(string dataKey, string dataValue)
        {
            DataKey = dataKey;
            DataValue = dataValue;
        }
        public string DataKey { get; set; }
        public string DataValue { get; set; }

    }
}
