using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace Lotek
{
    [Serializable]
    public class LuckyNumbers
    {
        public List<List<int>> Numbers { get; set; }
    }
}
