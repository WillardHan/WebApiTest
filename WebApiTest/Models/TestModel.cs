using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiTest.Models
{
    public class Test1
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class Test2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TestSetting
    {
        public int ParaA { get; set; }
        public string ParaB { get; set; }
    }
}
